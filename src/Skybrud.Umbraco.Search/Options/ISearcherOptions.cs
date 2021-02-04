using Examine;

namespace Skybrud.Umbraco.Search.Options {

    /// <summary>
    /// Extends the <see cref="ISearchOptions"/> interface with a <see cref="Searcher"/> property.
    /// </summary>
    public interface ISearcherOptions : ISearchOptions {

        /// <summary>
        /// Gets a reference to the searcher to be used for the search.
        /// </summary>
        ISearcher Searcher { get; }

    }

}