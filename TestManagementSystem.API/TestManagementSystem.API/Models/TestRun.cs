namespace TestManagementSystem.API.Models
{
    public class TestRun
    {
        public int Id { get; set; }
        public int TestCaseId { get; set; }
        public int ExecutedById { get; set; }
        public string Status { get; set; } = "Running";
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TestCase TestCase { get; set; } = null!;
        public User ExecutedBy { get; set; } = null!;
        public ICollection<TestRunResult> TestRunResults { get; set; } = new List<TestRunResult>();
    }
}
