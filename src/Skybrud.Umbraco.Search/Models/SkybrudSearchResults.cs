using System.Collections.Generic;
using Examine;
using Skybrud.Umbraco.Search.Options;

namespace Skybrud.Umbraco.Search.Models {

    /// <summary>
    /// Class representing a result of a search based on <see cref="Options"/>.
    /// </summary>
    public class SkybrudSearchResults {

        /// <summary>
        /// Gets the options the search was based on.
        /// </summary>
        public ISearchOptions Options { get; }

        /// <summary>
        /// Gets whether debugging was enabled for the search.
        /// </summary>
        public bool IsDebug { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Gets the total amount of items returned by the search.
        /// </summary>
        public long Total { get; }

        /// <summary>
        /// Gets the items returned by the search.
        /// </summary>
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