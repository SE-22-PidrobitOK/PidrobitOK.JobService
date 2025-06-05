using System.ComponentModel.DataAnnotations.Schema;

namespace JobService.Models
{
    public enum JobType { PartTime = 0, FullTime = 1, Internship = 2 }
    public enum JobStatus { Open = 0, Closed = 1 }

    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Location { get; set; }
        public decimal? Salary { get; set; }
        public JobType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid EmployerId { get; set; }
        public JobStatus Status { get; set; } = JobStatus.Open;
        public int RequiredExperience { get; set; }

        public ICollection<JobRequiredSkill> RequiredSkills { get; set; } = new List<JobRequiredSkill>();
    }
}
