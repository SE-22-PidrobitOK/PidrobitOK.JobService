using AutoMapper;
using JobService.DatabaseContext;
using JobService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobService.Repositories.JobsRepository
{
    public class JobRepository : IJobRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public JobRepository(ApplicationDbContext db, IMapper mapper)
        {
            _dbContext = db; _mapper = mapper;
        }

        public async Task<JobDto> Retrieve(Guid id)
        {
            var job = await _dbContext.Jobs.FindAsync(id);
            return _mapper.Map<JobDto>(job);
        }

        public async Task<List<JobDto>> Retrieve()
        {
            var jobs = await _dbContext.Jobs.ToListAsync();
            return _mapper.Map<List<JobDto>>(jobs);
        }

        public async Task<JobDto> Insert(JobDto jobDto)
        {
            jobDto.Status = JobStatus.Open;
            var job = _mapper.Map<Job>(jobDto);
            
            await _dbContext.Jobs.AddAsync(job);
            await _dbContext.SaveChangesAsync();

            jobDto.Id = job.Id;
            return jobDto;
        }

        public async Task<JobDto> Update(JobDto jobDto)
        {
            var existing = await _dbContext.Jobs.FindAsync(jobDto.Id);
            
            if (existing == null)
            {
                return null;
            }

            var job = _mapper.Map<Job>(jobDto);

            _dbContext.Entry(existing).CurrentValues.SetValues(jobDto);
            await _dbContext.SaveChangesAsync();
            return jobDto;
        }

        public async Task<bool> Delete(Guid id)
        {
            var job = await _dbContext.Jobs.FirstOrDefaultAsync(x => x.Id == id);
            if (job is null) return false;
            _dbContext.Jobs.Remove(job);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}