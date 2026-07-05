namespace TestManagementSystem.API.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public int? BugId { get; set; }
        public int? TestCaseId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public int UploadedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Bug? Bug { get; set; }
        public TestCase? TestCase { get; set; }
        public User UploadedBy { get; set; } = null!;
    }
}
