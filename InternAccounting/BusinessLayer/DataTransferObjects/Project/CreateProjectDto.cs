using System.ComponentModel.DataAnnotations;

namespace InternAccounting.BusinessLayer.DataTransferObjects.Project
{
    public class CreateProjectDto
    {
        [Required(ErrorMessage = "Название обязятельно"), MaxLength(100)]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов.")]
        public string Description { get; set; }

        [Range(1, 100, ErrorMessage = "Количество слотов должно быть от 1 до 100.")]
        public int SlotsCount { get; set; }
    }
}
