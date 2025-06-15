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
    /// Gets or sets the zero-based index of the current page.
    /// </summary>
    /// <value>The zero-based page index.</value>
    public int PageIndex { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <value>The page size.</value>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    /// <value>The total count of items.</value>
    public long TotalCount { get; set; }
}