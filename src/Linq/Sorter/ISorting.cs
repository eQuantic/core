namespace eQuantic.Core.Linq.Sorter
{
    public interface ISorting
    {
        string ColumnName { get; set; }
        SortDirection SortDirection { get; set; }
    }
}