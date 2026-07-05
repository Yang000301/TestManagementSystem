namespace TestManagementSystem.API.DTOs.TestCase
{
    public class CreateTestCaseDto
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Priority { get; set; } = "Medium";
    }
}
