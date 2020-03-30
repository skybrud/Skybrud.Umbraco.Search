using System.Collections.Generic;
using System.Linq;
using Examine;
using Skybrud.Essentials.Strings.Extensions;
using Skybrud.Umbraco.Search.Options.Fields.Conditions;

namespace Skybrud.Umbraco.Search.Options.Groups {

    public class SearchGroupOption : ISearchGroup {

        #region Properties

        /// <summary>
        /// Gets the numeric ID of the group. 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets an array of content types that results in this group should match.
        /// </summary>
        public string[] ContentTypes { get; }

        /// <summary>
        /// Gets or sets a parent IDs that results in this group should match (be a descendant of). This filter is OR-based, meaning that at least one of the specified parent IDs should be matched.
        /// </summary>
        public int[] ParentIds { get; set; }

        /// <summary>
        /// Gets a list of field conditions the returned results should match.
        /// </summary>
        public FieldConditionList FieldConditions { get; set; }

        /// <summary>
        /// Gets the type of the group.
        /// </summary>
        public SearchGroupType Type { get; }

        /// <summary>
        /// Gets or sets whether the group is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        #endregion

        #region Constructors

        public SearchGroupOption(int id, string name, string[] contentTypes, int parentId) {
            Id = id;
            Name = name;
            ContentTypes = contentTypes ?? new string[0];
            ParentIds = parentId > 0 ? new[]{ parentId } : new int[0];
            FieldConditions = new FieldConditionList();
            Type = SearchGroupType.Default;
        }

        public SearchGroupOption(int id, string name, string[] contentTypes, int parentId, SearchGroupType type) {
            Id = id;
            Name = name;
            ContentTypes = contentTypes ?? new string[0];
            ParentIds = parentId > 0 ? new[] { parentId } : new int[0];
            FieldConditions = new FieldConditionList();
            Type = type;
        }

        public SearchGroupOption(int id, string name, string[] contentTypes) {
            Id = id;
            Name = name;
            ContentTypes = contentTypes ?? new string[0];
            ParentIds = new int[0];
            FieldConditions = new FieldConditionList();
            Type = SearchGroupType.Default;
        }

        public SearchGroupOption(int id, string name, string[] contentTypes, FieldConditionList fieldConditions) {
            Id = id;
            Name = name;
            ContentTypes = contentTypes ?? new string[0];
            ParentIds = new int[0];
            FieldConditions = fieldConditions ?? new FieldConditionList();
            Type = SearchGroupType.Default;
        }

        public SearchGroupOption(int id, string name, string[] contentTypes, int[] parentIds) {
            Id = id;
            Name = name;
            ContentTypes = contentTypes ?? new string[0];
            ParentIds = parentIds ?? new int[0];
            FieldConditions = new FieldConditionList();
            Type = SearchGroupType.Default;
        }

        public SearchGroupOption(int id, string name, IEnumerable<string> contentTypes, IEnumerable<int> parentIds) {
            Id = id;
            Name = name;
            ContentTypes = contentTypes?.ToArray() ?? new string[0];
            ParentIds = parentIds?.ToArray() ?? new int[0];
            FieldConditions = new FieldConditionList();
            Type = SearchGroupType.Default;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Returns whether the specified <paramref name="result"/> is a match in this group.
        /// </summary>
        /// <param name="result">The result to check.</param>
        /// <returns><c>true</c> if the result is a match, otherwise <c>false</c>.</returns>
        public bool IsMatch(SearchResult result) {

            if (ContentTypes.Length > 0 && result.Fields.TryGetValue("nodeTypeAlias", out string nodeTypeAlias)) {
                if (ContentTypes.Contains(nodeTypeAlias) == false) return false;
            }

            if (ParentIds != null && ParentIds.Length > 0) {

                // Return "false" if the "path_search" field isn't present
                if (result.Fields.TryGetValue("path_search", out string path) == false) return false;

                // Return false if the parent ID is not part of the path
                if (ParentIds.Any(x => path.ToInt32Array().Contains(x)) == false) return false;

            }

            if (FieldConditions != null && FieldConditions.Count > 0) {

                // All field conditions should match, so if at least one condition doesn't match, we can exclude the result
                if (FieldConditions.Any(x => x.IsMatch(result) == false)) return false;

            }

            return true;

        }

        #endregion

    }

}