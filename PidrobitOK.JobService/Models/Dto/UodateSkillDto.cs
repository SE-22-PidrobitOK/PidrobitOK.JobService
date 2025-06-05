namespace JobService.Models
{
    public class UpdateSkillDto
    {
        public Guid SkillId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
