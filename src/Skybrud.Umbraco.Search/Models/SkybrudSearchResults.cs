using System.Collections.Generic;
using Examine;
using Skybrud.Umbraco.Search.Options;

namespace Skybrud.Umbraco.Search {

    public class SkybrudSearchResults {

        public ISearchOptions Options { get; }

        public long Total { get; }

        public IEnumerable<ISearchResult> Results { get; }

        public SkybrudSearchResults(ISearchOptions options, long total, IEnumerable<ISearchResult> results) {
            Options = options;
            Total = total;
            Results = results;
        }

    }

}