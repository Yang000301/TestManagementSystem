using System.ComponentModel.DataAnnotations;

namespace TestManagementSystem.API.DTOs.Project
{
    // 更新project
    public class UpdateProjectDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
