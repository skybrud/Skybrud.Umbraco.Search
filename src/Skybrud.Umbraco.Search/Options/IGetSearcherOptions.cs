using Examine;

namespace Skybrud.Umbraco.Search.Options {

    /// <summary>
    /// Extends the <see cref="ISearchOptions"/> interface with a <see cref="GetSearcher"/> method.
    /// </summary>
    public interface IGetSearcherOptions : ISearchOptions {

        /// <summary>
        /// Returns the <see cref="ISearcher"/> to be used for the search.
        /// </summary>
        /// <param name="examineManager">An instance of <see cref="IExamineManager"/>.</param>
        /// <param name="searchHelper">An instance of <see cref="ISearchHelper"/>.</param>
        /// <returns>The <see cref="ISearcher"/> to be used for the search.</returns>
        ISearcher GetSearcher(IExamineManager examineManager, ISearchHelper searchHelper);

    }

}