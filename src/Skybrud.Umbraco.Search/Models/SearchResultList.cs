using System.Collections.Generic;
using Examine;
using Examine.Search;
using Skybrud.Umbraco.Search.Options;
using Skybrud.Umbraco.Search.Options.Pagination;

namespace Skybrud.Umbraco.Search.Models {

    /// <summary>
    /// Class representing a result of a search based on <see cref="Options"/>.
    /// </summary>
    public class SearchResultList {

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
        public IQuery Query { get; }

        /// <summary>
        /// Gets the total amount of items returned by the search.
        /// </summary>
        public long Total { get; }

        /// <summary>
        /// Gets the items returned by the search.
        ///
        /// If <see cref="Options"/> implements <see cref="IOffsetOptions"/>, the items will be paginated honouring
        /// <see cref="IOffsetOptions.Limit"/> and <see cref="IOffsetOptions.Offset"/>.
        /// </summary>
        public IEnumerable<ISearchResult> Items { get; }

        #endregion

        #region Constructors

        public SearchResultList(ISearchOptions options, IQuery query, long total, IEnumerable<ISearchResult> items) {
            Options = options;
            IsDebug = options is IDebugSearchOptions debug && debug.IsDebug;
            Query = query;
            Total = total;
            Items = items;
        }

        #endregion

    }

}