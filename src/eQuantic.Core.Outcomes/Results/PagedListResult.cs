using System.Linq;
using System.Reflection;
using eQuantic.Core.Collections;
using Newtonsoft.Json;

namespace eQuantic.Core.Outcomes.Results
{
    public class PagedListResult<T> : ListResult<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }

        [JsonProperty("__next")]
        public Metadata Next { get; set; }

        [JsonProperty("__previows")]
        public Metadata Previows { get; set; }

        public bool HaveNext => PageIndex * PageSize < TotalCount;
        public bool HavePreviows => (PageIndex * PageSize) - PageSize > 0;


        public PagedListResult() : base()
        {

        }

        
        public PagedListResult(IPagedEnumerable<T> items) : this()
        {
            AddPagedItems(items);
        }

        public void AddPagedItems(IPagedEnumerable<T> items)
        {
            PageIndex = items.PageIndex;
            PageSize = items.PageSize;
            TotalCount = items.TotalCount;
            Items = items.ToList();
        }

        public PagedList<T> ToPagedList()
        {
            return new PagedList<T>(Items, TotalCount) {PageIndex = PageIndex, PageSize = PageSize};
        }
    }
}