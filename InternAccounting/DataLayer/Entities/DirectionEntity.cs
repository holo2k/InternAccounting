namespace InternAccounting.DataLayer.Entities
{
    public class DirectionEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SlotsCount { get; set; } = 0;
        public virtual ICollection<InternEntity> Interns { get; set; } = new List<InternEntity>();
    }
}
