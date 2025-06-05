namespace JobService.Models.Dto
{
    public class CreateJobDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public decimal? Salary { get; set; }
        public JobType Type { get; set; }
        public Guid EmployerId { get; set; }
        public int RequiredExperience { get; set; }
    }
}
