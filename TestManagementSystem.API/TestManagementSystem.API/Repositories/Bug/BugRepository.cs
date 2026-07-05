using TestManagementSystem.API.Data;
using Microsoft.EntityFrameworkCore;
namespace TestManagementSystem.API.Repositories.Bug
{
    public class BugRepository : IBugRepository
    {
        private readonly AppDbContext _context;
        public BugRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Bug>> GetByTestCaseIdAsync(int testCaseId)
        {
            return await _context.Bugs
                .Where(b => b.TestCaseId == testCaseId)
                .ToListAsync();
        }
        public async Task<Models.Bug?> GetByIdAsync(int id)
        {
            return await _context.Bugs
                .Include(b => b.TestCase)
                .Include(b => b.ReportedBy)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Models.Bug> CreateAsync(Models.Bug bug)
        {
            _context.Bugs.Add(bug);
            await _context.SaveChangesAsync();
            return bug;
        }

        public async Task<Models.Bug> UpdateAsync(Models.Bug bug)
        {
            _context.Bugs.Update(bug);
            await _context.SaveChangesAsync();
            return bug;
        }

        public async Task DeleteAsync(Models.Bug bug)
        {
            _context.Bugs.Remove(bug);
            await _context.SaveChangesAsync();
        }
    }
}
