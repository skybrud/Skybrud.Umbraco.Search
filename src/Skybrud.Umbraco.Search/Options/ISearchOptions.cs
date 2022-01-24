using Examine;
using Examine.Search;

namespace Skybrud.Umbraco.Search.Options {

    public interface ISearchOptions {

        /// <summary>
        /// Returns a boolean operator for an Examine search.
        /// </summary>
        /// <param name="searchHelper">The searcher helper.</param>
        /// <param name="searcher">The Examine searcher.</param>
        /// <param name="query">The Examine query.</param>
        /// <returns>An instance of <see cref="IQuery"/>.</returns>
        IBooleanOperation GetBooleanOperation(ISearchHelper searchHelper, ISearcher searcher, IQuery query);

    }

}