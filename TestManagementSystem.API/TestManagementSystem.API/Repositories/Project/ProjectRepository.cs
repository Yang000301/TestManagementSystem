using Microsoft.EntityFrameworkCore;
using TestManagementSystem.API.Data;

namespace TestManagementSystem.API.Repositories.Project
{
    public class ProjectRepository: IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Project>> GetAllAsync(int userId)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Where(p => p.OwnerId == userId ||
                    p.ProjectMembers.Any(pm => pm.UserId == userId))
                .ToListAsync();
        }

        public async Task<Models.Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Models.Project> CreateAsync(Models.Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task<Models.Project> UpdateAsync(Models.Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        public async Task DeleteAsync(Models.Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}

