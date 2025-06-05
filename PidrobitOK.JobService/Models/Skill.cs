using System.ComponentModel.DataAnnotations.Schema;

namespace JobService.Models
{
    public class Skill
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid SkillId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<JobRequiredSkill> JobRequiredSkills { get; set; } = new();
    }
}
