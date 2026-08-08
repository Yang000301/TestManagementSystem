using TestManagementSystem.API.DTOs.TestCase;

namespace TestManagementSystem.API.Services.TestCase
{
    public interface ITestCaseService
    {
        Task<List<TestCaseResponseDto>?> GetByProjectIdAsync(int projectId, int userId);
        Task<TestCaseResponseDto?> GetByIdAsync(int id, int userId);
        Task<TestCaseResponseDto> CreateAsync(CreateTestCaseDto dto, int userId);
        Task<TestCaseResponseDto> UpdateAsync(int id, UpdateTestCaseDto dto, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
