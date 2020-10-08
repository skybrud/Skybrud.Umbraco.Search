using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class SearchGroupResultList {

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("total")]
        public long Total { get; }

        [JsonProperty("limit")]
        public int Limit { get; }

        [JsonProperty("offset")]
        public long Offset { get; }

        [JsonProperty("items")]
        public IEnumerable<object> Items { get; }

        public SearchGroupResultList(SearchGroup group, int limit, long offset, long total, IEnumerable<object> items) {
            Id = group.Id;
            Name = group.Name;
            Total = total;
            Limit = limit;
            Offset = offset;
            Items = items;
        }

    }

}