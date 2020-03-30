namespace Skybrud.Umbraco.Search.Options.Groups {

    public interface IGroupedSearchOptions : ISearchOptions {

        /// <summary>
        /// Gets the groups of the search.
        /// </summary>
        SearchGroupList Groups { get; }

        /// <summary>
        /// Gets or sets the method used for determining the groups each result belongs to.
        /// </summary>
        SearchGroupMethod GroupMethod { get; }

    }

}