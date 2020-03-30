using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Examine;
using Newtonsoft.Json;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Search.Models.Interfaces;
using Skybrud.Umbraco.Search.Models.Options;
using Skybrud.Umbraco.Search.Options.Fields.Conditions;
using Skybrud.Umbraco.Search.Options.Groups;
using Umbraco.Core.Logging;

namespace Skybrud.Umbraco.Search.Models {

    public class SearchGroup {

        #region Properties

        [JsonProperty("id")]
        public int Id { get; }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("groupTitle")]
        [Obsolete("Use the Name property instead.")]
        public string GroupTitle => Name;

        /// <summary>
        /// Gets an array of content types the results of this group should match.
        /// </summary>
        [JsonIgnore]
        public string[] ContentTypes { get; set; }

        /// <summary>
        /// Gets whether this group specifies one or more content types that should be matched.
        /// </summary>
        [JsonIgnore]
        public bool HasContentTypes => ContentTypes != null && ContentTypes.Length > 0;

        /// <summary>
        /// Gets the ID of the node of which the returned results should be a descendant. The value must be a positive number for this filter to apply.
        /// </summary>
        [JsonIgnore]
        public int ParentId { get; set; }

        /// <summary>
        /// Gets a list of field conditions the returned results should match.
        /// </summary>
        [JsonIgnore]
        public FieldConditionList FieldConditions { get; set; }

        [JsonIgnore]
        public bool HasFieldConditions => FieldConditions != null && FieldConditions.Count > 0;

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonIgnore]
        public bool MiscResults { get; set; }

        [JsonIgnore]
        public List<SearchResult> AssociatedResults { get; set; }

        [JsonIgnore]
        public IEnumerable<SearchResult> SearcedAssociatedResults { get; set; }

        #endregion

        #region Constructors

        public SearchGroup(int id, string name, string[] contentTypes, int parentId, FieldConditionList fieldConditions, bool miscResults) {
            Id = id;
            Name = name;
            ContentTypes = contentTypes;
            ParentId = parentId;
            FieldConditions = fieldConditions;
            Count = 0;
            MiscResults = miscResults;
            AssociatedResults = new List<SearchResult>();
        }

        #endregion

        #region Static Members

        private static bool IsMatch(SearchGroup group, SearchResult result) {

            if (group.HasContentTypes) {
                if (result.Fields.TryGetValue("nodeTypeAlias", out string nodeTypeAlias) == false) return false;
                if (group.ContentTypes.Contains(nodeTypeAlias) == false) return false;
            }

            // All conditions of the group should match, so we can return "false" if at least one condition doesn't match
            if (group.HasFieldConditions && group.FieldConditions.Any(x => x.IsMatch(result) == false)) return false;

            if (group.ParentId > 0) {
                if (result.Fields.TryGetValue("path_search", out string path) == false) return false;
                if (StringUtils.ParseInt32Array(path).Contains(group.ParentId) == false) return false;
            }

            return true;

        }
        
        public static void SortSearchGroupsAsSearchResults(ref List<SearchGroup> groups, ref IEnumerable<SearchResult> results) {

            SearchGroup misc;

            foreach (SearchGroup group in groups) {

                // Skip the miscellaneous group for now as we handle that individually
                if (group.MiscResults) {
                    misc = group;
                    continue;
                }

                List<SearchResult> hest = new List<SearchResult>();

                foreach (var result in results) {

                    if (IsMatch(group, result)) {
                        group.AssociatedResults.Add(result);
                        group.Count++;
                    }

                }

            }

        }

        public static List<SearchResult> SearchInGroupsAsSearchResults(int[] searchInGroup, Order order, ref List<SearchGroup> groups, out int total)
        {
            var groupResults = new List<SearchResult>();

            foreach (var i in searchInGroup)
            {
                if (groups.Any(x=>x.Id == i))
                {
                    groupResults.AddRange(groups.First(x => x.Id == i).AssociatedResults);

                }
            }
            groups.ForEach(x => x.AssociatedResults.Clear());
            total = groupResults.Count;

            return Order.OrderResultSet(groupResults, order).ToList();
        }
        public static List<SearchResult> SearchInGroupsAsSearchResultsBySearching(int[] searchInGroup, Order order, ref List<SearchGroup> groups, out int total)
        {
            var groupResults = new List<SearchResult>();

            foreach (var i in searchInGroup)
            {
                if (groups.Any(x => x.Id == i))
                {
                    groupResults.AddRange(groups.First(x => x.Id == i).AssociatedResults);

                }
            }
            groups.ForEach(x => x.AssociatedResults.Clear());
            total = groupResults.Count;

            return Order.OrderResultSet(groupResults, order).ToList();
        }

        #endregion

        public static void SortSearchGroupsAsSearchResultsBySearching(ref List<SearchGroup> groups, int total, IExamineOptions searchOptions)
        {
            if (searchOptions != null)
            {
                int miscCount = 0;
                foreach (var sg in groups)
                {
                    if (sg.MiscResults) continue;

                    SearchOptions so = searchOptions as SearchOptions;
                    so.Debug = false;
                    
                    int tempTotal = 0;
                    
                    //DOCTYPE SEARCH
                 
                    so.DocumentTypes = sg.ContentTypes;
                 
                    //FIELD CONDITIONS
                    if (sg.FieldConditions != null && sg.FieldConditions.Any())
                    {
                        var sf = so.SpecificFields.Fields;
                        foreach (var sgFc in sg.FieldConditions)
                        {
                            sf.Add(SpecificField.GetSpecificField(sgFc.Alias, sgFc.Value.Split(new[] { ' ' })));
                        }
                    }
                    else
                    {
                        so.SpecificFields.Fields.Clear();

                    }

                    //PARENT CHECK
                    so.RootIds = sg.ParentId > 0 ? new[] { sg.ParentId } : null;

                    //SEARCH IT 
                    var results =SkybrudSearch.SearchExamine(so, out tempTotal);
                 
                    miscCount += tempTotal; 

                    sg.Count = tempTotal;
                    sg.SearcedAssociatedResults = results;

                }
                foreach (var sg in groups.Where(x => x.MiscResults)) // har sit eget loop, da det ikke er sikkert den er til sidst.
                {
                    sg.Count += total - miscCount;
                }

            }
        }
    }
}