using System.Collections.Generic;
using Examine;
using Skybrud.Umbraco.Search.Options;

namespace Skybrud.Umbraco.Search.Models {

    public class SkybrudSearchResults {

        public ISearchOptions Options { get; }

        public bool IsDebug { get; }

        public string Query { get; }

        public long Total { get; }

        public IEnumerable<ISearchResult> Results { get; }

        public SkybrudSearchResults(ISearchOptions options, string query, long total, IEnumerable<ISearchResult> results) {
            Options = options;
            IsDebug = options.IsDebug;
            Query = query;
            Total = total;
            Results = results;
        }

    }

}