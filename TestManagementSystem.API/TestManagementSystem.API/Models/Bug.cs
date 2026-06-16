using System.Net.Mail;

namespace TestManagementSystem.API.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public int TestCaseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = "Medium";
        public string Status { get; set; } = "Open";
        public int ReportedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TestCase TestCase { get; set; } = null!;
        public User ReportedBy { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
