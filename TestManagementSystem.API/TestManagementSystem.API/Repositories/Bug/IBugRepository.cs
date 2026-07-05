

namespace TestManagementSystem.API.Repositories.Bug

{
    public interface IBugRepository
    {
        Task<List<Models.Bug>> GetByTestCaseIdAsync(int testCaseId);
        Task<Models.Bug?> GetByIdAsync(int id);
        Task<Models.Bug> CreateAsync(Models.Bug bug);
        Task<Models.Bug> UpdateAsync(Models.Bug bug);
        Task DeleteAsync(Models.Bug bug);
    }
}
