using Examine;

namespace Skybrud.Umbraco.Search.Options.Groups  {
    
    /// <summary>
    /// Interface representing a search group.
    /// </summary>
    public interface ISearchGroup {

        /// <summary>
        /// Gets the numeric ID of the group. 
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets the type of the group.
        /// </summary>
        SearchGroupType Type { get; }

        /// <summary>
        /// Gets or sets whether the group is selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Returns whether the specified <paramref name="result"/> is a match in this group.
        /// </summary>
        /// <param name="result">The result to check.</param>
        /// <returns><c>true</c> if the result is a match, otherwise <c>false</c>.</returns>
        bool IsMatch(SearchResult result);

    }

}