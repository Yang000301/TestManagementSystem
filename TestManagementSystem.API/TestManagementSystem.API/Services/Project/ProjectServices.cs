using TestManagementSystem.API.DTOs.Project;
using TestManagementSystem.API.Repositories.Project;

namespace TestManagementSystem.API.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<ProjectResponseDto>> GetAllAsync(int userId)
        {
            var projects = await _projectRepository.GetAllAsync(userId);
            return projects.Select(MapToDto).ToList();
        }

        public async Task<ProjectResponseDto?> GetByIdAsync(int id, int userId)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return null;
            return MapToDto(project);
        }

        public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto, int userId)
        {
            var project = new Models.Project
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = userId,
                Status = "Active"
            };

            var created = await _projectRepository.CreateAsync(project);
            return MapToDto(created);
        }

        public async Task<ProjectResponseDto> UpdateAsync(int id, UpdateProjectDto dto, int userId)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new Exception("專案不存在");
            if (project.OwnerId != userId)
                throw new Exception("沒有權限修改此專案");

            project.Name = dto.Name;
            project.Description = dto.Description;
            project.Status = dto.Status;

            var updated = await _projectRepository.UpdateAsync(project);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new Exception("專案不存在");
            if (project.OwnerId != userId)
                throw new Exception("沒有權限刪除此專案");

            await _projectRepository.DeleteAsync(project);
        }

        private ProjectResponseDto MapToDto(Models.Project project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Status = project.Status,
                OwnerId = project.OwnerId,
                OwnerName = project.Owner?.Username ?? string.Empty,
                CreatedAt = project.CreatedAt
            };
        }
    }
}
