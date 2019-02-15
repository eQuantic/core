using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace eQuantic.Core.Outcomes.Results
{
    public class ListResult<T> : BasicResult
    {
        public virtual List<T> Items { get; set; }

        [JsonProperty("__metadata")]
        public Metadata Metadata { get; set; }

        public ListResult()
        {
            Items = new List<T>();
        }
        public ListResult(IEnumerable<T> items)
        {
            Items = items.ToList();
        }
    }
}
