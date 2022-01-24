using System.Collections.Generic;
using Examine;
using Microsoft.Extensions.Logging;

namespace Skybrud.Umbraco.Search.Options.Sorting {

    /// <summary>
    /// Interface used for decribing how a collection of <see cref="ISearchResult"/> should be sorted after the search has been executed.
    /// </summary>
    public interface IPostSortOptions : ISearchOptions {

        IEnumerable<ISearchResult> Sort(IEnumerable<ISearchResult> results, ILogger logger);

    }

}