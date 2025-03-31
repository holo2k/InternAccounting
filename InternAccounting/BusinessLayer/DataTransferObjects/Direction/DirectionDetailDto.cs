using InternAccounting.BusinessLayer.DataTransferObjects.Intern;

namespace InternAccounting.BusinessLayer.DataTransferObjects.Direction
{
    public class DirectionDetailDto : DirectionDto
    {
        public List<InternShortDto> Interns { get; set; } = new();
    }
}
