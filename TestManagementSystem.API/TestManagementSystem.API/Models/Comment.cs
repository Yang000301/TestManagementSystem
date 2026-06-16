namespace TestManagementSystem.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int? BugId { get; set; }
        public int? TestCaseId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Bug? Bug { get; set; }
        public TestCase? TestCase { get; set; }
    }
}
