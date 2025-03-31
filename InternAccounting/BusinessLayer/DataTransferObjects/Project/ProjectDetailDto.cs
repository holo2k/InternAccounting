using InternAccounting.BusinessLayer.DataTransferObjects.Intern;

namespace InternAccounting.BusinessLayer.DataTransferObjects.Project
{
    public class ProjectDetailDto : ProjectDto
    {
        public List<InternShortDto> Interns { get; set; } = new();
    }
}
