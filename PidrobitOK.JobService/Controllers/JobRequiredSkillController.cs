using AutoMapper;
using JobService.Models;
using JobService.Repositories;
using JobService.Repositories.JobRequiredSkillsRepository;
using JobService.Repositories.JobsRepository;
using Microsoft.AspNetCore.Mvc;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRequiredSkillController : ControllerBase
    {
        private readonly IJobRequiredSkillRepository _repo;
        private readonly IJobRepository _jobrepo;
        private readonly ISkillRepository _skillrepo;
        private readonly IMapper _mapper;

        public JobRequiredSkillController(IJobRequiredSkillRepository repo, IMapper mapper, IJobRepository jobrepo, ISkillRepository skillrepo)
        {
            _repo = repo;
            _mapper = mapper;
            _jobrepo = jobrepo;
            _skillrepo = skillrepo;
        }

        [HttpGet("retrieve-all")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repo.Retrieve());

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJobRequiredSkillDto dto)
        {
            // Check if the job and skill exist
            var job = await _jobrepo.Retrieve(dto.JobId);
            if (job == null)
                return NotFound($"Job with ID {dto.JobId} not found");

            var skill = await _skillrepo.Retrieve(dto.SkillId);
            if (skill == null)
                return NotFound($"Skill with ID {dto.SkillId} not found");
            var exists = await _repo.Exists(dto.JobId, dto.SkillId);
            // Check if the job already requires this skill
            if (exists)
                return Conflict("This job already requires this skill.");


            var entity = _mapper.Map<JobRequiredSkillDto>(dto);
            var created = await _repo.Insert(entity);
            return Ok(created);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id) => Ok(await _repo.Delete(id));


        // GET /api/job-required-skill/skills-by-job?jobId=...
        [HttpGet("skills-by-job")]
        public async Task<IActionResult> GetSkillsByJob([FromQuery] Guid jobId)
        {
            var result = await _repo.GetSkillsByJobId(jobId);
            return Ok(result);
        }

        //GET /api/job-required-skill/jobs-by-skill? skillId = ...
        [HttpGet("jobs-by-skill")]
        public async Task<IActionResult> GetJobsBySkill([FromQuery] Guid skillId)
        {
            var result = await _repo.GetJobsBySkillId(skillId);
            return Ok(result);
        }

    }
}
