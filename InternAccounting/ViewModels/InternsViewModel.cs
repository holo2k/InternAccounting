using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using InternAccounting.BusinessLayer.Filter;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InternAccounting.ViewModels
{
    public class InternsViewModel
    {
        public InternFilterModel Filter { get; set; }
        public PaginatedList<InternDto> Interns { get; set; }
        public IEnumerable<SelectListItem> Directions { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public IEnumerable<SelectListItem> Genders { get; set; }
        public AddInternViewModel AddIntern { get; set; } = new AddInternViewModel();
    }
}
