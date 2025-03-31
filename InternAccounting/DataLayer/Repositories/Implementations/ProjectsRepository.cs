using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DAL;
using InternAccounting.DataLayer.Entities;
using InternAccounting.DataLayer.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace InternAccounting.DataLayer.Repositories.Implementations
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly AppDbContext _context;

        public ProjectsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<ProjectEntity>> GetFilteredProjectsAsync(FilterModel filter)
        {
            var query = _context.Projects
                .Include(p => p.Interns)
                .AsQueryable();

            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                query = query.Where(p => p.Title.Contains(filter.SearchString) ||
                                    p.Description.Contains(filter.SearchString));
            }

            query = filter.SortField switch
            {
                "title_desc" => query.OrderByDescending(p => p.Title),
                "interns_asc" => query.OrderBy(p => p.Interns.Count),
                "interns_desc" => query.OrderByDescending(p => p.Interns.Count),
                _ => query.OrderBy(p => p.Title), // title_asc по умолчанию
            };

            return await PaginatedList<ProjectEntity>.CreateAsync(query, filter.PageNumber, filter.PageSize);
        }

        public async Task<ProjectEntity> GetProjectWithInternsAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Interns)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddProjectAsync(ProjectEntity project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProjectAsync(ProjectEntity project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ProjectHasInternsAsync(int id)
        {
            return await _context.Projects
                .AnyAsync(p => p.Id == id && p.Interns.Any());
        }
    }
}
