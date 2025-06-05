using AutoMapper;
using JobService.Models;
using JobService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISkillRepository _skillRepository;

        public SkillController(IMapper mapper, ISkillRepository skillRepository)
        {
            _mapper = mapper;
            _skillRepository = skillRepository;
        }

        [HttpGet("retrieve")]
        public async Task<IActionResult> Get([FromQuery] Guid id)
        {
            var skill = await _skillRepository.Retrieve(id);
            return Ok(skill);
        }

        [HttpGet("retrieve-all")]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _skillRepository.Retrieve();
            return Ok(skills);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
        {
            var exists = await _skillRepository.ExistsByName(dto.Title);

            // Check if the skill already exists
            if (exists)
                return Conflict("Skill with this name already exists.");

            var skill = _mapper.Map<SkillDto>(dto);
            var created = await _skillRepository.Insert(skill);
            
            return Ok(created);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSkillDto dto)
        {
            var skill = _mapper.Map<SkillDto>(dto);
            var updated = await _skillRepository.Update(skill);
            if (updated == null) return NotFound();
            
            return Ok(updated);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _skillRepository.Delete(id);
            return Ok(result);
        }
    }
}