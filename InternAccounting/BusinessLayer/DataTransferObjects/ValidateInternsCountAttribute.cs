using InternAccounting.BusinessLayer.DataTransferObjects.Direction;
using System.ComponentModel.DataAnnotations;

namespace InternAccounting.BusinessLayer.DataTransferObjects
{
    public class ValidateInternsCountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var dto = context.ObjectInstance as UpdateDirectionDto;
            if (dto?.InternIds?.Count > dto.SlotsCount)
            {
                return new ValidationResult("Количество стажеров превышает доступные слоты");
            }
            return ValidationResult.Success;
        }
    }
}
