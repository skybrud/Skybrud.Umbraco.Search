using Examine;

namespace Skybrud.Umbraco.Search.Options  {

    public interface ISearchOptions {

        /// <summary>
        /// Gets a reference to the searcher to be used for the search.
        /// </summary>
        ISearcher Searcher { get; }

        /// <summary>
        /// Gets the raw query for the search.
        /// </summary>
        /// <returns></returns>
        string GetRawQuery();

    }

}