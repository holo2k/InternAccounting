using InternAccounting.BusinessLayer.DataTransferObjects.Project;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.DataLayer.Entities;

namespace InternAccounting.BusinessLayer.Services.Abstractions
{
    public interface IProjectService
    {
        public Task<PaginatedList<ProjectDto>> GetProjectsAsync(FilterModel filter);
        public Task<ProjectDetailDto> GetProjectDetailsAsync(int id);
        public Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto);
        public Task UpdateProjectAsync(int id, UpdateProjectDto dto);
        public Task DeleteProjectAsync(int id);
        public Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();

    }
}
