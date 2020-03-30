using System;
using System.Collections.Generic;
using System.ComponentModel;
using Examine;
using Newtonsoft.Json;
using Skybrud.Umbraco.Search.Options.Groups;

namespace Skybrud.Umbraco.Search.Models.Grouping {

    public class SearchGroup {

        #region Properties

        /// <summary>
        /// Gets the numeric ID of the group. 
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets the title of the group.
        /// </summary>
        [JsonProperty("groupTitle")]
        public string Title { get; set; }

        /// <summary>
        /// Alias of <see cref="ContentTypes"/>.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Use the ContentTypes property instead.")]
        public string[] Doctypes {
            get => ContentTypes;
            set => ContentTypes = value;
        }

        /// <summary>
        /// Gets or sets an array of content types that results in this group should match.
        /// </summary>
        [JsonIgnore]
        public string[] ContentTypes { get; set; }

        /// <summary>
        /// Gets or sets a parent ID that results in this group should match (be a descendant of).
        /// </summary>
        [JsonIgnore]
        public string ParentId { get; set; }

        [JsonIgnore]
        [Description("Each key corresponds to a field int the idex, the value must be an exact match of the fields value")]
        public List<KeyValuePair<string, string>> FieldConditions { get; set; }

        //public List<FieldCondition> FieldConditions { get; set; } // TODO: skift FieldConditions til at bruge denne

        /// <summary>
        /// Gets the amount of matched results in the group.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Returns <c>true</c> if the <see cref="Type"/> property is <see cref="SearchGroupType.Misc"/>.
        /// </summary>
        [JsonIgnore]
        [Obsolete("Use the Type property instead.")]
        public bool MiscResults => Type == SearchGroupType.Misc;
        
        /// <summary>
        /// Gets the type of the group.
        /// </summary>
        [JsonIgnore]
        public SearchGroupType Type { get; }

        [JsonIgnore]
        public List<SearchResult> AssociatedResults { get; set; }

        #endregion

        public SearchGroup(int id, string title, string[] contentTypes, string parentId, List<KeyValuePair<string, string>> fieldConditions, bool miscResults) {
            Id = id;
            Title = title;
            ContentTypes = contentTypes;
            ParentId = parentId;
            FieldConditions = fieldConditions;
            Count = 0;
            Type = miscResults ? SearchGroupType.Misc : SearchGroupType.Default;
            AssociatedResults = new List<SearchResult>();
        }

        public SearchGroup(int id, string groupTitle, string[] doctypes) : this(id, groupTitle, doctypes, null, null, false) { }

    }

}