using AutoMapper;
using JobService;
using JobService.DatabaseContext;
using JobService.Repositories;
using JobService.Repositories.JobRequiredSkillsRepository;
using JobService.Repositories.JobsRepository;
using JobService.Repositories.SkillsRepository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var ownEnvironment = builder.Environment.EnvironmentName;
var connectionString = string.Empty;

if (builder.Environment.IsDevelopment())
{
    var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
    Env.Load(envPath);

    connectionString = $"Data Source=DESKTOP-BKNC2M0;Database={Environment.GetEnvironmentVariable("SQL_DATABASE")};Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
}


else if (ownEnvironment == "DockerDevelopment")
{
    connectionString = $"Server={Environment.GetEnvironmentVariable("SQL_SERVER")};" +
    $"Database={Environment.GetEnvironmentVariable("SQL_DATABASE")};" +
    $"User Id={Environment.GetEnvironmentVariable("SQL_USER")};" +
    $"Password={Environment.GetEnvironmentVariable("SQL_PASSWORD")};" +
    "TrustServerCertificate=True";
}


// Override database connection
// 1. EF
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
var tokenLifetime = int.Parse(Environment.GetEnvironmentVariable("JWT_TOKEN_LIFETIME"));


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobService", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Format: \"Bearer {your JWT token}\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<ISkillRepository, SkillRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobRequiredSkillRepository, JobRequiredSkillRepository>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
    };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
