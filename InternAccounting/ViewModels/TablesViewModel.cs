using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using InternAccounting.BusinessLayer.DataTransferObjects.Project;
using InternAccounting.BusinessLayer.Filter;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternAccounting.ViewModels
{
    public class TablesViewModel
    {
        public string? ActiveTab { get; set; } = "directions";

        public FilterModel? DirectionsFilter { get; set; } = new FilterModel();
        public PaginatedList<DirectionDto>? Directions { get; set; }

        public FilterModel? ProjectsFilter { get; set; } = new FilterModel();
        public PaginatedList<ProjectDto>? Projects { get; set; }

        public CreateDirectionDto? NewDirection { get; set; }
        public CreateProjectDto? NewProject { get; set; }

        public List<SelectListItem>? AvailableInterns { get; set; }

        public string? SelectedProjectInternIds { get; set; }

        [NotMapped]
        public List<int>? SelectedProjectInternIdsList =>
            !string.IsNullOrEmpty(SelectedProjectInternIds)
                ? SelectedProjectInternIds.Split(',').Select(int.Parse).ToList()
                : new List<int>();

        public string? SelectedDirecitonInternIds { get; set; }

        [NotMapped]
        public List<int>? SelectedDirectionInternIdsList =>
            !string.IsNullOrEmpty(SelectedDirecitonInternIds)
                ? SelectedDirecitonInternIds.Split(',').Select(int.Parse).ToList()
                : new List<int>();

        public int? EditDirectionId { get; set; }
        public int? EditProjectId { get; set; }
    }

    
}
