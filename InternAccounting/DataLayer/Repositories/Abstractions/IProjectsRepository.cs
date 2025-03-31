using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace InternAccounting.DataLayer.Repositories.Abstractions
{
    public interface IProjectsRepository
    {
        Task<PaginatedList<ProjectEntity>> GetFilteredProjectsAsync(FilterModel filter);
        Task<ProjectEntity> GetProjectWithInternsAsync(int id);
        Task AddProjectAsync(ProjectEntity project);
        Task UpdateProjectAsync(ProjectEntity project);
        Task DeleteProjectAsync(int id);
        Task<bool> ProjectHasInternsAsync(int id);
    }
}
