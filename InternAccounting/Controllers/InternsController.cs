using AutoMapper;
using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.DataTransferObjects.Project;
using InternAccounting.BusinessLayer.Filter;
using InternAccounting.BusinessLayer.Services.Abstractions;
using InternAccounting.BusinessLayer.Services.Implementations;
using InternAccounting.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InternAccounting.Controllers
{
    public class InternsController : Controller
    {
        private readonly IInternService _internService;
        private readonly IDirectionService _directionService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public InternsController(
            IInternService internService,
            IDirectionService directionService,
            IProjectService projectService,
            IMapper mapper)
        {
            _internService = internService;
            _directionService = directionService;
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(InternFilterModel filter)
        {
            var directions = await _directionService.GetAllDirectionsAsync();
            var projects = await _projectService.GetAllProjectsAsync();
            var interns = await _internService.GetInternsAsync(filter);

            var viewModel = new InternsViewModel
            {
                Interns = interns,
                Filter = filter,
                Directions = directions.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Title
                }),
                Projects = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title
                }),
                Genders = Enum.GetValues(typeof(Gender)).Cast<Gender>()
                    .Select(g => new SelectListItem
                    {
                        Value = g.ToString(),
                        Text = g.ToString()
                    })
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var interns = await _internService.GetInternsAsync(new InternFilterModel());
               
                return Json(interns);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInternForEdit(int id)
        {
            try
            {
                var intern = await _internService.GetInternDetailsAsync(id);
                var viewModel = _mapper.Map<AddInternViewModel>(intern);

                var directions = await _directionService.GetAllDirectionsAsync();
                var projects = await _projectService.GetAllProjectsAsync();

                viewModel.Directions = directions.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Title,
                    Selected = d.Id == viewModel.DirectionId
                });

                viewModel.Projects = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Title,
                    Selected = p.Id == viewModel.ProjectId
                });

                return Json(new
                {
                    id = viewModel.Id,
                    firstName = viewModel.FirstName,
                    lastName = viewModel.LastName,
                    email = viewModel.Email,
                    phoneNumber = viewModel.PhoneNumber,
                    birthDate = viewModel.BirthDate.ToString("yyyy-MM-dd"),
                    directionId = viewModel.DirectionId,
                    projectId = viewModel.ProjectId,
                    sex = viewModel.Sex,
                    directions = viewModel.Directions,
                    projects = viewModel.Projects
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetIntern(int id)
        {
            var intern = await _internService.GetInternDetailsAsync(id);
            if (intern == null) return NotFound();

            return Json(_mapper.Map<InternDetailDto>(intern));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddInternViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await RepopulateDropdowns(viewModel);
                return View("Index", await CreateInternsViewModel());
            }

            try
            {
                var dto = _mapper.Map<CreateInternDto>(viewModel);
                await _internService.CreateInternAsync(dto);
                TempData["Success"] = "Стажёр успешно добавлен";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                viewModel = await RepopulateDropdowns(viewModel);
                return View("Index", await CreateInternsViewModel());
            }

           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddInternViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = await RepopulateDropdowns(viewModel);
                return View("Index", await CreateInternsViewModel());
            }

            try
            {
                var dto = _mapper.Map<CreateInternDto>(viewModel);
                await _internService.UpdateInternAsync(id, dto);
                TempData["Success"] = "Стажёр успешно обновлён";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                viewModel = await RepopulateDropdowns(viewModel);
                return View("Index", await CreateInternsViewModel());
            }
        }
        private async Task<AddInternViewModel> RepopulateDropdowns(AddInternViewModel viewModel)
        {
            var directions = await _directionService.GetAllDirectionsAsync();
            var projects = await _projectService.GetAllProjectsAsync();

            viewModel.Directions = directions.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Title
            });

            viewModel.Projects = projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Title
            });

            return viewModel;
        }

        private async Task<InternsViewModel> CreateInternsViewModel()
        {
            return new InternsViewModel
            {
                Interns = await _internService.GetInternsAsync(new InternFilterModel())
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _internService.DeleteInternAsync(id);
            TempData["Success"] = "Стажёр успешно удалён";
            return RedirectToAction(nameof(Index));
        }

    }
}
