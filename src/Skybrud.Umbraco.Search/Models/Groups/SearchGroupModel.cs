using System.Collections.Generic;
using Examine;
using Newtonsoft.Json;
using Skybrud.Umbraco.Search.Options.Groups;

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class SearchGroupModel {

        private readonly int _count;

        /// <summary>
        /// Gets a reference to the <see cref="ISearchGroup"/> this model is based on.
        /// </summary>
        [JsonIgnore]
        public ISearchGroup Group { get; }

        /// <summary>
        /// Gets the numeric ID of the group. 
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; }
        
        [JsonProperty("count")]
        public int Count => Results?.Count ?? _count;

        [JsonProperty("selected")]
        public bool IsSelected { get; }

        [JsonIgnore]
        public List<SearchResult> Results { get; }

        public SearchGroupModel(ISearchGroup group) {
            Group = group;
            Id = group.Id;
            Name = group.Name;
            IsSelected = group.IsSelected;
            Results = new List<SearchResult>();
        }

        public SearchGroupModel(SearchGroup group, bool selected) {
            Id = group.Id;
            Name = group.Name;
            IsSelected = selected;
            _count = group.Count;
        }

    }

}