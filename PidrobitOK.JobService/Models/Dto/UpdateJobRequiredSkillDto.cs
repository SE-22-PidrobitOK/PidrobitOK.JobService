namespace JobService.Models
{
    public class UpdateJobRequiredSkillDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid SkillId { get; set; }
    }
}
