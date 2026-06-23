namespace TestManagementSystem.API.Repositories.TestCase
{
    public interface ITestCaseRepository
    {
        Task<List<Models.TestCase>> GetByProjectIdAsync(int projectId);
        Task<Models.TestCase?> GetByIdAsync(int id);
        Task<Models.TestCase> CreateAsync(Models.TestCase testCase);
        Task<Models.TestCase> UpdateAsync(Models.TestCase testCase);
        Task DeleteAsync(Models.TestCase testCase);
    }   
}
