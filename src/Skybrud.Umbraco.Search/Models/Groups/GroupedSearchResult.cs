using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class GroupedSearchResult {

        [JsonProperty("groups")]
        public SearchGroupResult[] Groups { get; }

        public GroupedSearchResult(IEnumerable<SearchGroupResult> groups) {
            Groups = groups.ToArray();
        }

    }

}