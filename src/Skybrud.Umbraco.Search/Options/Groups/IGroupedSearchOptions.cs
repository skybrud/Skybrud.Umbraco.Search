namespace Skybrud.Umbraco.Search.Options.Groups {

    public interface IGroupedSearchOptions : ISearchOptions {

        /// <summary>
        /// Gets the groups of the search.
        /// </summary>
        SearchGroupList Groups { get; }

    }

}