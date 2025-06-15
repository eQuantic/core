using System.Collections;
using System.Collections.Generic;

namespace eQuantic.Core.Collections;

/// <summary>
/// Represents a paged enumerable collection with pagination metadata.
/// </summary>
public interface IPagedEnumerable : IEnumerable
{
    /// <summary>
    /// Gets or sets the zero-based index of the current page.
    /// </summary>
    /// <value>The zero-based page index.</value>
    int PageIndex { get; set; }
    
    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    /// <value>The page size.</value>
    int PageSize { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    /// <value>The total count of items.</value>
    long TotalCount { get; set; }
}

/// <summary>
/// Represents a strongly-typed paged enumerable collection with pagination metadata.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public interface IPagedEnumerable<T> : IPagedEnumerable, IEnumerable<T>
{
        
}