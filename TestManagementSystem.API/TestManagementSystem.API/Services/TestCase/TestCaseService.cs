using TestManagementSystem.API.DTOs.TestCase;
using TestManagementSystem.API.Repositories.Project;
using TestManagementSystem.API.Repositories.TestCase;

namespace TestManagementSystem.API.Services.TestCase
{
    public class TestCaseService : ITestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly IProjectRepository _projectRepository;

        public TestCaseService(
    ITestCaseRepository testCaseRepository,
    IProjectRepository projectRepository)
        {
            _testCaseRepository = testCaseRepository;
            _projectRepository = projectRepository;
        }

        public async Task<List<TestCaseResponseDto>?> GetByProjectIdAsync(int projectId,int userId)
        {
            var project =
                await _projectRepository.GetByIdAsync(projectId, userId);

            if (project == null)
                return null;

            var testCases =
                await _testCaseRepository.GetByProjectIdAsync(projectId);

            return testCases.Select(MapToDto).ToList();
        }

        public async Task<TestCaseResponseDto?> GetByIdAsync(int id, int userId)
        {
            var testCase = await _testCaseRepository.GetByIdAsync(id);

            if (testCase == null)
                return null;

            var project =
                await _projectRepository.GetByIdAsync(testCase.ProjectId, userId);

            if (project == null)
                return null;

            return MapToDto(testCase);
        }

        public async Task<TestCaseResponseDto> CreateAsync(
    CreateTestCaseDto dto,
    int userId)
        {
            var project =
                await _projectRepository.GetByIdAsync(dto.ProjectId, userId);

            if (project == null)
                throw new Exception("專案不存在或沒有權限新增測試案例");

            var testCase = new Models.TestCase
            {
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                Status = "Pending",
                CreatedById = userId
            };

            var created =
                await _testCaseRepository.CreateAsync(testCase);

            return MapToDto(created);
        }

        public async Task<TestCaseResponseDto> UpdateAsync(
    int id,
    UpdateTestCaseDto dto,
    int userId)
        {
            var testCase =
                await _testCaseRepository.GetByIdAsync(id);

            if (testCase == null)
                throw new Exception("測試案例不存在");

            var project =
                await _projectRepository.GetByIdAsync(testCase.ProjectId, userId);

            if (project == null)
                throw new Exception("專案不存在或沒有權限");

            if (testCase.CreatedById != userId)
                throw new Exception("沒有權限修改此測試案例");

            testCase.Title = dto.Title;
            testCase.Description = dto.Description;
            testCase.Priority = dto.Priority;
            testCase.Status = dto.Status;

            var updated =
                await _testCaseRepository.UpdateAsync(testCase);

            return MapToDto(updated);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var testCase =
                await _testCaseRepository.GetByIdAsync(id);

            if (testCase == null)
                throw new Exception("測試案例不存在");

            var project =
                await _projectRepository.GetByIdAsync(testCase.ProjectId, userId);

            if (project == null)
                throw new Exception("專案不存在或沒有權限");

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
