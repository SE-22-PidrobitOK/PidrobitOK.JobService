using AutoMapper;
using JobService.DatabaseContext;
using JobService.Models;
using Microsoft.EntityFrameworkCore;

namespace JobService.Repositories.JobRequiredSkillsRepository
{
    public class JobRequiredSkillRepository : IJobRequiredSkillRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public JobRequiredSkillRepository(ApplicationDbContext db, IMapper mapper)
        {
            _dbContext = db; 
            _mapper = mapper;
        }

        public async Task<List<JobRequiredSkillDto>> Retrieve()
        {
            var list = await _dbContext.JobRequiredSkills.ToListAsync();
            return _mapper.Map<List<JobRequiredSkillDto>>(list);
        }

        public async Task<JobRequiredSkillDto> Insert(JobRequiredSkillDto entity)
        {
            var jobRequredSkill = _mapper.Map<JobRequiredSkill>(entity);
            await _dbContext.JobRequiredSkills.AddAsync(jobRequredSkill);
            await _dbContext.SaveChangesAsync();

            entity.Id = jobRequredSkill.Id;
            return entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await _dbContext.JobRequiredSkills.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null) return false;
            _dbContext.JobRequiredSkills.Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<SkillDto>> GetSkillsByJobId(Guid jobId)
        {
            var skills = await _dbContext.JobRequiredSkills
                .Where(jrs => jrs.JobId == jobId)
                .Include(jrs => jrs.Skill)
                .Select(jrs => jrs.Skill!)
                .ToListAsync();

            return _mapper.Map<List<SkillDto>>(skills);
        }

        public async Task<List<JobDto>> GetJobsBySkillId(Guid skillId)
        {
            var jobs = await _dbContext.JobRequiredSkills
                .Where(jrs => jrs.SkillId == skillId)
                .Include(jrs => jrs.Job)
                .Select(jrs => jrs.Job!)
                .ToListAsync();

            return _mapper.Map<List<JobDto>>(jobs);
        }

        public async Task<bool> Exists(Guid jobId, Guid skillId)
        {
            return await _dbContext.JobRequiredSkills.AnyAsync(jrs =>
                jrs.JobId == jobId && jrs.SkillId == skillId);
        }
    }
}