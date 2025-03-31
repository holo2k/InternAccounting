namespace InternAccounting.BusinessLayer.Filter
{
    public class InternFilterModel : FilterModel
    {
        public int? DirectionId { get; set; }
        public int? ProjectId { get; set; }
        public string? Gender { get; set; }
    }
}
