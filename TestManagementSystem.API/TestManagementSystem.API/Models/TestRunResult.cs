namespace TestManagementSystem.API.Models
{
    public class TestRunResult
    {
        public int Id { get; set; }
        public int TestRunId { get; set; }
        public string Result { get; set; } = "Pending";
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public TestRun TestRun { get; set; } = null!;
    }
}
