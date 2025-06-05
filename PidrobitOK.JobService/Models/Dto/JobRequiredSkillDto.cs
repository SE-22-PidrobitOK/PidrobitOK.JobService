namespace JobService.Models
{
    public class JobRequiredSkillDto
    {
        public Guid Id { get; set; }
        public Guid JobId { get; set; }
        public Guid SkillId { get; set; }
    }
}
