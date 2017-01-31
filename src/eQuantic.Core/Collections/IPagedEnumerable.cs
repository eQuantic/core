using System.Collections;
using System.Collections.Generic;

namespace eQuantic.Core.Collections
{
    public interface IPagedEnumerable : IEnumerable
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int TotalCount { get; set; }
    }
    public interface IPagedEnumerable<T> : IPagedEnumerable, IEnumerable<T>
    {
        
    }
}