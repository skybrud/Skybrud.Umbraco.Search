using System.Collections.Generic;
using Examine;
using Examine.Search;

namespace Skybrud.Umbraco.Search.Options.Sorting {
    
    /// <summary>
    /// Interface describing a <see cref="Execute"/> method.
    /// </summary>
    public interface IExecuteOptions : ISearchOptions {

        void Execute(IQueryExecutor executor, out IEnumerable<ISearchResult> results, out long total);

    }

}