using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class GroupedSearchResult {

        [JsonProperty("groups")]
        public SearchGroupResultList[] Groups { get; }

        public GroupedSearchResult(IEnumerable<SearchGroupResultList> groups) {
            Groups = groups.ToArray();
        }

    }

}