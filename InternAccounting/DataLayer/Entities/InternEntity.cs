using InternAccounting.BusinessLayer.DataTransferObjects.Intern;

namespace InternAccounting.DataLayer.Entities
{
    public class InternEntity
    {
        public int Id { get; set; }
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required Gender Sex { get; set; }
        public required Email Email { get; set; }
        public PhoneNumber? PhoneNumber { get; set; }
        public required DateTime BirthDate {  get; set; }

        public int? DirectionId { get; set; }
        public virtual DirectionEntity Direction { get; set; }

        public int? ProjectId { get; set; }
        public virtual ProjectEntity Project { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
