using System.Data;
using JobService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobService.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Skill> Skills { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobRequiredSkill> JobRequiredSkills { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.Type)
                      .HasConversion<string>();

                entity.Property(e => e.Status)
                      .HasConversion<string>();
            });

            modelBuilder.Entity<JobRequiredSkill>()
                .HasKey(jrs => jrs.Id);

            modelBuilder.Entity<JobRequiredSkill>()
                .HasOne(jrs => jrs.Job)
                .WithMany(j => j.RequiredSkills)
                .HasForeignKey(jrs => jrs.JobId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobRequiredSkill>()
                .HasOne(jrs => jrs.Skill)
                .WithMany()
                .HasForeignKey(jrs => jrs.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
