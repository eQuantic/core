using System.Collections.Generic;

namespace eQuantic.Core.Collections;

/// <summary>
/// Represents a paged list that implements IPagedEnumerable interface.
/// </summary>
/// <typeparam name="T">The type of elements in the list.</typeparam>
public class PagedList<T> : List<T>, IPagedEnumerable<T>
{
    /// <summary>
    /// Initializes a new instance of the PagedList class.
    /// </summary>
    /// <param name="collection">The collection of items to include in this page.</param>
    /// <param name="total">The total number of items across all pages.</param>
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