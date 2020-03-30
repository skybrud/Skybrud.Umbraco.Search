using System;
using System.Collections.Generic;
using Examine;
using Newtonsoft.Json;
using Skybrud.Umbraco.Search.Options.Groups;

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class SearchGroupModel {

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

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        [JsonProperty("groupTitle")]
        [Obsolete("Use Name instead.")]
        public string GroupTitle => Name;

        [JsonProperty("count")]
        public int Count => Results.Count;

        [JsonProperty("selected")]
        public bool IsSelected { get; }

        [JsonIgnore]
        public List<SearchResult> Results { get; }

        public SearchGroupModel(ISearchGroup group) {
            Id = group.Id;
            Name = group.Name;
            IsSelected = group.IsSelected;
            Results = new List<SearchResult>();
        }

    }

}