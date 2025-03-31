namespace InternAccounting.BusinessLayer.DataTransferObjects.Intern
{
    public class InternDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string DirectionName { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
