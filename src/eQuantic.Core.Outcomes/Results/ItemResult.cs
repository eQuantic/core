using Newtonsoft.Json;

namespace eQuantic.Core.Outcomes.Results
{
    public class ItemResult<TItem> : BasicResult
    {
        public TItem Item { get; set; }

        [JsonProperty("__list")]
        public Metadata ListMetadata { get; set; }
    }
}