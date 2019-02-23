namespace eQuantic.Core.Linq
{
    public interface ISorting
    {
        string ColumnName { get; set; }
        bool Ascending { get; set; }
    }
}
