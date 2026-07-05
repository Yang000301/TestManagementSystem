using TestManagementSystem.API.DTOs.Project;

namespace TestManagementSystem.API.Services.Project
{
    public interface IProjectService
    {
        Task<List<ProjectResponseDto>> GetAllAsync(int userId);
        Task<ProjectResponseDto?> GetByIdAsync(int id, int userId);
        Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto, int userId);
        Task<ProjectResponseDto> UpdateAsync(int id, UpdateProjectDto dto, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
