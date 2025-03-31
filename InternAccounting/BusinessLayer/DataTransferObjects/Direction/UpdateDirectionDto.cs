using System.ComponentModel.DataAnnotations;

namespace InternAccounting.BusinessLayer.DataTransferObjects.Direction
{
    public class UpdateDirectionDto : CreateDirectionDto
    {
        [ValidateInternsCount]
        public List<int> InternIds { get; set; } = new();
    }

    
}
