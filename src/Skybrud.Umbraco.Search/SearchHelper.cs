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
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Skybrud.Umbraco.Search {

    public class SearchHelper {

        private readonly IExamineManager _examine;

        private readonly ILogger _logger;

        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        #region Constructors

        public SearchHelper(IExamineManager examine, ILogger logger, IUmbracoContextAccessor umbracoContextAccessor) {
            _examine = examine;
            _logger = logger;
            _umbracoContextAccessor = umbracoContextAccessor;
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
        /// </summary>
        /// <param name="options">The options for the search.</param>
        /// <returns>An instance of <see cref="SearchResultList"/> representing the result of the search.</returns>
        public virtual SearchResultList Search(ISearchOptions options) {

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
            return new SearchResultList(options, query, total, results);

        }

        /// <summary>
        /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
        ///
        /// Each item in the result is parsed to the type of <typeparamref name="TItem"/> using <paramref name="callback"/>.
        /// </summary>
        /// <typeparam name="TItem">The common output type of each item.</typeparam>
        /// <param name="options">The options for the search.</param>
        /// <param name="callback">A callback used for converting an <see cref="ISearchResult"/> to <typeparamref name="TItem"/>.</param>
        /// <returns>An instance of <see cref="SearchResultList{TItem}"/> representing the result of the search.</returns>
        public virtual SearchResultList<TItem> Search<TItem>(ISearchOptions options, Func<ISearchResult, TItem> callback) {
            SearchResultList results = Search(options);
            return new SearchResultList<TItem>(options, results.Query, results.Total, results.Items.Select(callback));
        }

        /// <summary>
        /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
        ///
        /// Each item in the result first found in either the content cache or media cache, and then parsed to the type of <typeparamref name="TItem"/> using <paramref name="callback"/>.
        /// </summary>
        /// <typeparam name="TItem">The common output type of each item.</typeparam>
        /// <param name="options">The options for the search.</param>
        /// <param name="callback">A callback used for converting an <see cref="IPublishedContent"/> to <typeparamref name="TItem"/>.</param>
        /// <returns>An instance of <see cref="SearchResultList{TItem}"/> representing the result of the search.</returns>
        public virtual SearchResultList<TItem> Search<TItem>(ISearchOptions options, Func<IPublishedContent, TItem> callback) {

            SearchResultList results = Search(options);

            IEnumerable<TItem> items = results.Items
                .Select(GetPublishedContentFromResult)
                .WhereNotNull()
                .Select(callback);

            return new SearchResultList<TItem>(options, results.Query, results.Total, items);

        }

        /// <summary>
        /// Performs a search using the specified <paramref name="options"/> and returns the result of that search.
        ///
        /// Each item in the result first found in either the content cache or media cache, and then parsed to the type of <typeparamref name="TItem"/> using <paramref name="callback"/>.
        /// </summary>
        /// <typeparam name="TItem">The common output type of each item.</typeparam>
        /// <param name="options">The options for the search.</param>
        /// <param name="callback">A callback used for converting an <see cref="IPublishedContent"/> to <typeparamref name="TItem"/>.</param>
        /// <returns>An instance of <see cref="SearchResultList{TItem}"/> representing the result of the search.</returns>
        public virtual SearchResultList<TItem> Search<TItem>(ISearchOptions options, Func<IPublishedContent, ISearchResult, TItem> callback) {

            SearchResultList results = Search(options);

            // Map the search results
            IEnumerable<TItem> items = (
                from x in results.Items
                let content = GetPublishedContentFromResult(x)
                where content != null
                select callback(content, x)
            );

            return new SearchResultList<TItem>(options, results.Query, results.Total, items);

        }

        /// <summary>
        /// Converts the specified <paramref name="result"/> into an instance of <see cref="IPublishedContent"/>.
        ///
        /// The method will look at the <c>__IndexType</c> to determine the type of the result, and then use the
        /// relevant published cache (eg. content or media) to lookup the <see cref="IPublishedContent"/> equivalent of
        /// <paramref name="result"/>.
        /// </summary>
        /// <param name="result">The result to look up.</param>
        /// <returns>An instance of <see cref="IPublishedContent"/>.</returns>
        protected virtual IPublishedContent GetPublishedContentFromResult(ISearchResult result) {

            string indexType = result.GetValues("__IndexType").FirstOrDefault();

            switch (indexType) {

                case "content":
                    return _umbracoContextAccessor.UmbracoContext.Content.GetById(int.Parse(result.Id));

                case "media":
                case "pdf":
                    return _umbracoContextAccessor.UmbracoContext.Media.GetById(int.Parse(result.Id));

                default:
                    return null;

            }

        }

        /// <summary>
        /// Performs a search based on the specified <paramref name="request"/>.
        /// </summary>
        /// <param name="request">The request the search should be based on.</param>
        /// <param name="groups">An array of groups to used for the search.</param>
        /// <returns>An instance of <see cref="GroupedSearchResult"/>.</returns>
        public virtual GroupedSearchResult Search(HttpRequestBase request, SearchGroup[] groups) {

            int[] selectedGroups = request.QueryString["groups"].ToInt32Array();

            IEnumerable<SearchGroupResultList> result = (
                from x in groups
                where selectedGroups.Length == 0 || selectedGroups.Contains(x.Id)
                select x?.Callback(x, request, null)
            );

            return new GroupedSearchResult(result);

        }

        /// <summary>
        /// Returns a <see cref="DateTime"/> parsed from the field with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="key">The key of the field.</param>
        /// <returns>An instance of <see cref="DateTime"/>.</returns>
        public virtual DateTime GetSortValueByDateTime(ISearchResult result, string key) {
            return SearchUtils.Sorting.GetSortValueByDateTime(result, key);
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