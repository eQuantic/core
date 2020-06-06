namespace eQuantic.Core.Linq.Filter
{
    public interface IFiltering
    {
        string ColumnName { get; set; }
        FilterOperator Operator { get; set; }
        string StringValue { get; set; }
    }
}