using Examine;
using Examine.Search;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Search.Options.Sorting {

    /// <summary>
    /// Interface describing a <see cref="Execute"/> method.
    /// </summary>
    public interface IExecuteOptions : ISearchOptions {

        void Execute(IBooleanOperation operation, out IEnumerable<ISearchResult> results, out long total);

    }

}