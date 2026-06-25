namespace TestManagementSystem.API.DTOs.Bug
{
    public class UpdateBugDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
