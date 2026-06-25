using TestManagementSystem.API.DTOs.Bug;
namespace TestManagementSystem.API.Services.Bug
{
    public interface IBugService
    {
        Task<List<BugResponseDto>> GetByTestCaseIdAsync(int testCaseId);
        Task<BugResponseDto?> GetByIdAsync(int id);
        Task<BugResponseDto> CreateAsync(CreateBugDto dto, int userId);
        Task<BugResponseDto> UpdateAsync(int id, UpdateBugDto dto, int userId);
        Task DeleteAsync(int id, int userId);
    }
}
