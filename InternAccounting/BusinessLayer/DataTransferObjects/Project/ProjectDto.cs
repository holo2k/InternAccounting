using InternAccounting.BusinessLayer.DataTransferObjects.Intern;

namespace InternAccounting.BusinessLayer.DataTransferObjects.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SlotsCount { get; set; }
        public int InternsCount { get; set; }
        public List<InternShortDto> Interns { get; set; }

    }
}
