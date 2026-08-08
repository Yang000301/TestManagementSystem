using System.ComponentModel.DataAnnotations;

namespace TestManagementSystem.API.DTOs.Project
{
    //創project
    public class CreateProjectDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }
    }
}
