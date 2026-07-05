using TestManagementSystem.API.DTOs.Bug;
using TestManagementSystem.API.DTOs.TestCase;
using TestManagementSystem.API.Models;
using TestManagementSystem.API.Repositories.Bug;
namespace TestManagementSystem.API.Services.Bug
{
    public class BugService : IBugService
    {
        private readonly IBugRepository _bugRepository;
       
        public BugService(IBugRepository bugRepository)
        {
            _bugRepository = bugRepository;
            
        }
        public async Task<List<BugResponseDto>> GetByTestCaseIdAsync(int testCaseId)
        {
            var bugs = await _bugRepository.GetByTestCaseIdAsync(testCaseId);
            return bugs.Select(MapToDto).ToList();
        }
        public async Task<BugResponseDto?> GetByIdAsync(int id )
        {
            var bug = await _bugRepository.GetByIdAsync(id);
            if (bug == null) return null;
            return MapToDto(bug);
        }
        public async Task<BugResponseDto> CreateAsync(CreateBugDto dto, int userId)
        {
            var bug = new Models.Bug
            {
                TestCaseId = dto.TestCaseId,
                Title = dto.Title,
                Description = dto.Description,
                Severity = dto.Severity,
                Status = "Open",
                ReportedById = userId,
            };

            var createdBug = await _bugRepository.CreateAsync(bug);
            return MapToDto(createdBug);
        }
        public async Task<BugResponseDto?> UpdateAsync(int id, UpdateBugDto dto, int userId)
        {
            var bug = await _bugRepository.GetByIdAsync(id);
            if (bug == null)
                throw new Exception("問題不存在");
            if (bug.ReportedById != userId)
                throw new Exception("沒有權限修改此 Bug");
            bug.Title = dto.Title;
            bug.Description = dto.Description;
            bug.Severity = dto.Severity;
            bug.Status = dto.Status;

            var updatedBug = await _bugRepository.UpdateAsync(bug);
            return MapToDto(updatedBug);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            var bug = await _bugRepository.GetByIdAsync(id);
            if (bug == null) 
                throw new Exception("Bug 不存在");
            if (bug.ReportedById != userId)
                throw new Exception("沒有權限刪除此 Bug");
            await _bugRepository.DeleteAsync(bug);
           
        }
        private BugResponseDto MapToDto(Models.Bug bug)
        {
            return new BugResponseDto
            {
                Id = bug.Id,
                TestCaseId = bug.TestCaseId,
                Title = bug.Title,
                Description = bug.Description,
                Severity = bug.Severity,
                Status = bug.Status,
                ReportedById = bug.ReportedById,
                ReportedByName = bug.ReportedBy?.Username ?? string.Empty,
                CreatedAt = bug.CreatedAt,
            };
        }
    }
}


                
           