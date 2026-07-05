using System.Net.Mail;

namespace TestManagementSystem.API.Models
{
    public class TestCase
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Pending";
        public int CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Project Project { get; set; } = null!;
        public User CreatedBy { get; set; } = null!;
        public ICollection<TestRun> TestRuns { get; set; } = new List<TestRun>();
        public ICollection<Bug> Bugs { get; set; } = new List<Bug>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
