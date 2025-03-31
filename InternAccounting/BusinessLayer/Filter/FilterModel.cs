namespace InternAccounting.BusinessLayer.Filter
{
    public class FilterModel
    {
        public string SearchString { get; set; } = "";
        public string SortField { get; set; } = "title_asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
