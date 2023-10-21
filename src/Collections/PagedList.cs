using System.Collections.Generic;

namespace eQuantic.Core.Collections;

public class PagedList<T> : List<T>, IPagedEnumerable<T>
{
    public PagedList(IEnumerable<T> collection, long total) : base(collection)
    {
        PageSize = (int)total;
        TotalCount = total;
    }

    /// <summary>
    /// One-based index of this subset within the superset.
    /// </summary>
    /// <value>
    /// One-based index of this subset within the superset.
    /// </value>
    public int PageIndex { get; set; }

    /// <summary>
    /// Maximum size any individual subset.
    /// </summary>
    /// <value>
    /// Maximum size any individual subset.
    /// </value>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of objects contained within the superset.
    /// </summary>
    /// <value>
    /// Total number of objects contained within the superset.
    /// </value>
    public long TotalCount { get; set; }
}