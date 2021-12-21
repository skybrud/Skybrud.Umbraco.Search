using Examine;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Search.Options.Sorting {

    /// <summary>
    /// Interface used for decribing how a collection of <see cref="ISearchResult"/> should be sorted after the search has been executed.
    /// </summary>
    public interface IPostSortOptions : ISearchOptions {

        IEnumerable<ISearchResult> Sort(IEnumerable<ISearchResult> results);

    }

}