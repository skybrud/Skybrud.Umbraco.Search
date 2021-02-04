using System.Collections.Generic;
using Skybrud.Umbraco.Search.Options;

namespace Skybrud.Umbraco.Search.Models {

    /// <summary>
    /// Class representing a result of a search based on <see cref="Options"/>.
    /// </summary>
    public class SearchResultList<T> {

        #region Properties

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
        public IEnumerable<T> Items { get; }

        #endregion

        #region Constructors

        public SearchResultList(ISearchOptions options, string query, long total, IEnumerable<T> items) {
            Options = options;
            IsDebug = options is IDebugSearchOptions debug && debug.IsDebug;
            Query = query;
            Total = total;
            Items = items;
        }

        #endregion

    }

}