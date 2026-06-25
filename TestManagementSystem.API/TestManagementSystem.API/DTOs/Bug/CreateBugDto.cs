namespace TestManagementSystem.API.DTOs.Bug
{
    public class CreateBugDto
    {
        public int TestCaseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = "Medium";
        public string ReportedById { get; set; }

    }
}
