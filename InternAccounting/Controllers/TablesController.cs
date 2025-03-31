using AutoMapper;
using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using InternAccounting.BusinessLayer.DataTransferObjects.Project;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.BusinessLayer.Services.Abstractions;
using InternAccounting.BusinessLayer.Services.Implementations;
using InternAccounting.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternAccounting.Controllers
{
    public class TablesController : Controller
    {
        private readonly IDirectionService _directionService;
        private readonly IInternService _internService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public TablesController(
            IDirectionService directionService,
            IProjectService projectService,
            IInternService internService,
            IMapper mapper)
        {
            _directionService = directionService;
            _projectService = projectService;
            _internService = internService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
                string activeTab = "directions",
                int directionsPage = 1,
                int projectsPage = 1,
                int pageSize = 10)
        {

            activeTab ??= Request.Query["activeTab"].FirstOrDefault();

            activeTab ??= HttpContext.Session.GetString("ActiveTab");

            activeTab ??= "directions";

            var interns = await _internService.GetInternsAsync(new InternFilterModel());
            var directionsFilter = new FilterModel
            {
                PageNumber = directionsPage,
                PageSize = pageSize
            };

            var projectsFilter = new FilterModel
            {
                PageNumber = projectsPage,
                PageSize = pageSize
            };

            var viewModel = new TablesViewModel
            {
                ActiveTab = activeTab,
                Directions = await _directionService.GetDirectionsAsync(directionsFilter),
                Projects = await _projectService.GetProjectsAsync(projectsFilter),
                AvailableInterns = interns.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.FullName
                }).ToList()
        };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(TablesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Directions = await _directionService.GetDirectionsAsync(model.DirectionsFilter);
                model.Projects = await _projectService.GetProjectsAsync(model.ProjectsFilter);
                model.AvailableInterns = await GetAvailableInterns();
                return View(model);
            }

            try
            {
                if (model.EditDirectionId.HasValue)
                {
                    var updateDto = _mapper.Map<UpdateDirectionDto>(model.NewDirection);
                    updateDto.InternIds = model.SelectedDirectionInternIdsList;
                    await _directionService.UpdateDirectionAsync(model.EditDirectionId.Value, updateDto);
                    TempData["Success"] = "Направление успешно обновлено";
                }
                else if (model.EditProjectId.HasValue)
                {
                    var updateDto = _mapper.Map<UpdateProjectDto>(model.NewProject);
                    updateDto.InternIds = model.SelectedProjectInternIdsList;
                    await _projectService.UpdateProjectAsync(model.EditProjectId.Value, updateDto);
                    TempData["Success"] = "Проект успешно обновлен";
                }
                else
                {
                    if (model.ActiveTab == "directions")
                    {
                        await _directionService.CreateDirectionAsync(model.NewDirection);
                        TempData["Success"] = "Новое направление добавлено";
                    }
                    else
                    {
                        await _projectService.CreateProjectAsync(model.NewProject);
                        TempData["Success"] = "Новый проект добавлен";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            TempData.Keep();

            return RedirectToAction(nameof(Index), new
            {
                activeTab = model.ActiveTab,
            });
        }

        private async Task<List<SelectListItem>> GetAvailableInterns()
        {
            var interns = await _internService.GetInternsAsync(new InternFilterModel());
            return interns.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = $"{i.FullName} ({i.Email})"
            }).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDirection(int id)
        {
            try
            {
                await _directionService.DeleteDirectionAsync(id);
                TempData["Success"] = "Направление удалено";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index), new { activeTab = "directions" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);
                TempData["Success"] = "Проект удален";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index), new { activeTab = "projects" });
        }

        [HttpGet]
        public async Task<IActionResult> GetDirection(int id)
        {
            var direction = await _directionService.GetDirectionDetailsAsync(id);
            if (direction == null) return NotFound();

            return Json(new
            {
                id = direction.Id,
                title = direction.Title,
                description = direction.Description,
                slotsCount = direction.SlotsCount,
                interns = direction.Interns
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _projectService.GetProjectDetailsAsync(id);
            if (project == null) return NotFound();

            return Json(new
            {
                id = project.Id,
                title = project.Title,
                description = project.Description,
                slotsCount = project.SlotsCount,
                interns = project.Interns
            });
        }
    }
}
