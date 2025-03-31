namespace InternAccounting.BusinessLayer.DataTransferObjects.Project
{
    public class UpdateProjectDto : CreateProjectDto
    {
        [ValidateInternsCount]
        public List<int> InternIds { get; set; } = new();
    }
}
