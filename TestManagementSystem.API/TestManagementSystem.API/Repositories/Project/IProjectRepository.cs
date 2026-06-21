namespace TestManagementSystem.API.Repositories.Project
{
    public interface IProjectRepository
    {
        Task<List<Models.Project>> GetAllAsync(int userId);
        Task<Models.Project?> GetByIdAsync(int id);
        Task<Models.Project> CreateAsync(Models.Project project);
        Task<Models.Project> UpdateAsync(Models.Project project);
        Task DeleteAsync(Models.Project project);
    }
}
