using AutoMapper;
using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.DataTransferObjects.Project;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.BusinessLayer.Services.Abstractions;
using InternAccounting.DataLayer.Entities;
using InternAccounting.DataLayer.Repositories.Abstractions;
using InternAccounting.DataLayer.Repositories.Implementations;

namespace InternAccounting.BusinessLayer.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IMapper _mapper;
        private readonly IInternsRepository _internsRepository;

        public ProjectService(IProjectsRepository projectsRepository, IInternsRepository internsRepository, IMapper mapper)
        {
            _projectsRepository = projectsRepository;
            _internsRepository = internsRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProjectDto>> GetProjectsAsync(FilterModel filter)
        {
            var projects = await _projectsRepository.GetFilteredProjectsAsync(filter);
            var items = _mapper.Map<List<ProjectDto>>(projects);

            return new PaginatedList<ProjectDto>(
                items,
                projects.Count,
                projects.PageIndex,
                projects.PageSize
            );
        }

        public async Task<ProjectDetailDto> GetProjectDetailsAsync(int id)
        {
            var project = await _projectsRepository.GetProjectWithInternsAsync(id);
            return new ProjectDetailDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                SlotsCount = project.SlotsCount,
                InternsCount = project.Interns.Count,
                Interns = _mapper.Map<List<InternShortDto>>(project.Interns)
            };
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
        {
            var project = _mapper.Map<ProjectEntity>(dto);
            await _projectsRepository.AddProjectAsync(project);
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task UpdateProjectAsync(int id, UpdateProjectDto dto)
        {
            var project = await _projectsRepository.GetProjectWithInternsAsync(id);
            _mapper.Map(dto, project);

           

            await UpdateProjectInternsAsync(project, dto.InternIds);

            await _projectsRepository.UpdateProjectAsync(project);
        }

        private async Task UpdateProjectInternsAsync(ProjectEntity project, List<int> internIds)
        {
            if (project.SlotsCount < internIds.Count)
            {
                throw new InvalidOperationException("Слоты заполнены");
            }
            // Получаем текущих и новых стажеров
            var currentInternIds = project.Interns.Select(i => i.Id).ToList();
            var internsToAdd = internIds.Except(currentInternIds);
            var internsToRemove = currentInternIds.Except(internIds);

            // Удаляем старых
            foreach (var internId in internsToRemove)
            {
                var intern = project.Interns.First(i => i.Id == internId);
                project.Interns.Remove(intern);
            }

            // Добавляем новых
            foreach (var internId in internsToAdd)
            {
                var intern = await _internsRepository.GetInternWithDetailsAsync(internId);
                project.Interns.Add(intern);
            }
        }

        public async Task DeleteProjectAsync(int id)
        {
            if (await _projectsRepository.ProjectHasInternsAsync(id))
            {
                throw new InvalidOperationException("Нельзя удалить проект с привязанными стажерами");
            }

            await _projectsRepository.DeleteProjectAsync(id);
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var filter = new FilterModel { PageSize = int.MaxValue };
            var result = await GetProjectsAsync(filter);
            return result;
        }
    }
}
