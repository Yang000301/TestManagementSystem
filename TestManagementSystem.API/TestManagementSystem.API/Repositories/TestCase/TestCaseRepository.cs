using Microsoft.EntityFrameworkCore;
using TestManagementSystem.API.Data;

namespace TestManagementSystem.API.Repositories.TestCase
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly AppDbContext _context;

        public TestCaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.TestCase>> GetByProjectIdAsync(int projectId)
        {
            return await _context.TestCases
                .Include(tc => tc.CreatedBy)
                .Where(tc => tc.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<Models.TestCase?> GetByIdAsync(int id)
        {
            return await _context.TestCases
                .Include(tc => tc.CreatedBy)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task<Models.TestCase> CreateAsync(Models.TestCase testCase)
        {
            _context.TestCases.Add(testCase);
            await _context.SaveChangesAsync();
            return testCase;
        }

        public async Task<Models.TestCase> UpdateAsync(Models.TestCase testCase)
        {
            _context.TestCases.Update(testCase);
            await _context.SaveChangesAsync();
            return testCase;
        }

        public async Task DeleteAsync(Models.TestCase testCase)
        {
            _context.TestCases.Remove(testCase);
            await _context.SaveChangesAsync();
        }
    }
}
