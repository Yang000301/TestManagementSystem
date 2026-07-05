using TestManagementSystem.API.DTOs.TestCase;
using TestManagementSystem.API.Repositories.TestCase;

namespace TestManagementSystem.API.Services.TestCase
{
    public class TestCaseService : ITestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public TestCaseService(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        public async Task<List<TestCaseResponseDto>> GetByProjectIdAsync(int projectId)
        {
            var testCases = await _testCaseRepository.GetByProjectIdAsync(projectId);
            return testCases.Select(MapToDto).ToList();
        }

        public async Task<TestCaseResponseDto?> GetByIdAsync(int id)
        {
            var testCase = await _testCaseRepository.GetByIdAsync(id);
            if (testCase == null) return null;
            return MapToDto(testCase);
        }

        public async Task<TestCaseResponseDto> CreateAsync(CreateTestCaseDto dto, int userId)
        {
            var testCase = new Models.TestCase
            {
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = "Pending",
                CreatedById = userId
            };

            var created = await _testCaseRepository.CreateAsync(testCase);
            return MapToDto(created);
        }

        public async Task<TestCaseResponseDto> UpdateAsync(int id, UpdateTestCaseDto dto, int userId)
        {
            var testCase = await _testCaseRepository.GetByIdAsync(id);
            if (testCase == null)
                throw new Exception("測試案例不存在");
            if (testCase.CreatedById != userId)
                throw new Exception("沒有權限修改此測試案例");

            testCase.Title = dto.Title;
            testCase.Description = dto.Description;
            testCase.Priority = dto.Priority;
            testCase.Status = dto.Status;

            var updated = await _testCaseRepository.UpdateAsync(testCase);
            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var testCase = await _testCaseRepository.GetByIdAsync(id);
            if (testCase == null)
                throw new Exception("測試案例不存在");
            if (testCase.CreatedById != userId)
                throw new Exception("沒有權限刪除此測試案例");

            await _testCaseRepository.DeleteAsync(testCase);
        }

        private TestCaseResponseDto MapToDto(Models.TestCase testCase)
        {
            return new TestCaseResponseDto
            {
                Id = testCase.Id,
                ProjectId = testCase.ProjectId,
                Title = testCase.Title,
                Description = testCase.Description,
                Priority = testCase.Priority,
                Status = testCase.Status,
                CreatedById = testCase.CreatedById,
                CreatedByName = testCase.CreatedBy?.Username ?? string.Empty,
                CreatedAt = testCase.CreatedAt
            };
        }
    }
}
