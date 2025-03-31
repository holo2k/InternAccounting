using System.ComponentModel.DataAnnotations;

namespace InternAccounting.BusinessLayer.DataTransferObjects.Intern
{
    public class CreateInternDto
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public Gender Sex { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public int? DirectionId { get; set; }
        public int? ProjectId { get; set; }
    }
}
