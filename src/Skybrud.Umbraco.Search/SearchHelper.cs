using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Examine;
using Skybrud.Essentials.Strings.Extensions;
using Skybrud.Umbraco.Search.Models;
using Skybrud.Umbraco.Search.Models.Groups;
using Skybrud.Umbraco.Search.Options;
using Skybrud.Umbraco.Search.Options.Pagination;
using Skybrud.Umbraco.Search.Options.Sorting;
using Umbraco.Core.Logging;

namespace Skybrud.Umbraco.Search {

    public class SearchHelper {

        private readonly IExamineManager _examine;
        private readonly ILogger _logger;

        #region Constructors

        public SearchHelper(IExamineManager examine, ILogger logger) {
            _examine = examine;
            _logger = logger;
        }

        #endregion

        #region Member methods

        public virtual SkybrudSearchResults Search(ISearchOptions options) {

            // Start measuring the elapsed time
            Stopwatch sw = Stopwatch.StartNew();

            // Get the searcher from the options
            ISearcher searcher = options.Searcher;

            // Fall back to the searcher of the external index if a searcher hasn't been specified
            if (options.Searcher == null) {

                if (_examine.TryGetIndex("ExternalIndex", out IIndex index) == false) throw new Exception("Examine index not found.");

                // Get the searcher from the index
                searcher = index.GetSearcher();
                if (searcher == null) throw new Exception("Examine searcher not found.");

            }

            // Get the raw query via the options
            string query = options.GetRawQuery();

            // Make the search in Examine
            ISearchResults allResults = searcher.CreateQuery().NativeQuery(query).Execute(int.MaxValue);

            long total = allResults.TotalItemCount;

            IEnumerable<ISearchResult> results = allResults;

            // If "options" implements the interface, results are sorted using the "Sort" method
            if (options is IPostSortOptions s) results = s.Sort(results);

            // If "options" implements implement the interface, the results are paginated
            if (options is IOffsetOptions o) results = results.Skip(o.Offset).Take(o.Limit);

            sw.Stop();

            if (options.IsDebug) {
                _logger.Debug<SearchHelper>("Search of type {Type} completed in {Milliseconds} with {Query}", options.GetType().FullName, sw.ElapsedMilliseconds, query);
            }

            // Wrap the results
            return new SkybrudSearchResults(options, query, total, results);

        }
        
        /// <summary>
        /// Performs a search based on the specified <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The request the search should be based on.</param>
        /// <param name="groups">An array of groups to used for the search.</param>
        /// <returns>An instance of <see cref="GroupedSearchResult"/>.</returns>
        public virtual GroupedSearchResult Search(HttpRequestBase request, SearchGroup[] groups) {

            int[] selectedGroups = request.QueryString["groups"].ToInt32Array();

            IEnumerable<SearchGroupResult> result = (
                from x in groups
                where selectedGroups.Length == 0 || selectedGroups.Contains(x.Id)
                select x?.Callback(x, request)
            );

            return new GroupedSearchResult(result);

        }

        public virtual DateTime GetSortValueByDateTime(ISearchResult result, string propertyAlias) {
            return SearchUtils.Sorting.GetSortValueByDateTime(result, propertyAlias);
        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> parsed from the <c>contentDate</c> field, or <c>createDate</c> if nothing else is specified.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>An instance of <see cref="DateTime"/>.</returns>
        public virtual DateTime GetSortValueByContentDate(ISearchResult result) {
            return SearchUtils.Sorting.GetSortValueByContentDate(result);
        }

        #endregion

    }

}