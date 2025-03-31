using InternAccounting.BusinessLayer.DataTransferObjects.Intern;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InternAccounting.ViewModels
{
    public class AddInternViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пол обязателен")]
        [Display(Name = "Пол")]
        public Gender Sex { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Номер телефона должен начинаться с +7 и содержать 11 цифр")]
        [Display(Name = "Телефон")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Направление")]
        public int? DirectionId { get; set; }

        [Display(Name = "Проект")]
        public int? ProjectId { get; set; }

        public IEnumerable<SelectListItem>? Directions { get; set; }
        public IEnumerable<SelectListItem>? Projects { get; set; }
    }
}
