namespace JobService.Models
{
    public class JobDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public decimal? Salary { get; set; }
        public JobType Type { get; set; }
        public Guid EmployerId { get; set; }
        public JobStatus Status { get; set; }
        public int RequiredExperience { get; set; }
    }
}
