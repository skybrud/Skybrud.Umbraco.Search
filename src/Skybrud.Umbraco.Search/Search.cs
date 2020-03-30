using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Examine;
using Examine.Providers;
using Examine.SearchCriteria;
using Newtonsoft.Json.Linq;
using Skybrud.Umbraco.Search.Models;
using Skybrud.Umbraco.Search.Models.Interfaces;
using Skybrud.Umbraco.Search.Options;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.Umbraco.Search
{
    public class SkybrudSearch
    {
        /// <summary>
        ///     Search in your Umbraco Documents
        /// </summary>
        /// <param name="total">Returns the total number of results</param>
        /// <param name="searchOptions">Options for the search</param>
        /// <returns></returns>
        public static IEnumerable<IPublishedContent> SearchDocuments(
            out int total,
            IExamineOptions searchOptions = null
            )
        {
            var results = SearchExamine(searchOptions, out total);

            var paginatedOptions = (IPaginatedExamineOptions)searchOptions;


            // Apply pagination if an instance of "IPaginatedExamineOptions")
            var hest = paginatedOptions == null || paginatedOptions.Limit <= 0 ? results : results.Skip(paginatedOptions.Offset).Take(paginatedOptions.Limit);

            // standard søgeresultat
            return hest.Select(a => UmbracoContext.Current.ContentCache.GetById(a.Id));
        }

        public static IEnumerable<IPublishedContent> SearchDocuments(
            out int total,
            ref List<SearchGroup> grouping,
            int[] searchInGroup = null,
            IExamineOptions searchOptions = null
            )
        {
            var results = SearchExamine(searchOptions, out total);

            IEnumerable<SearchResult> tempResults = results;  // arbejdskopi af resultater

            //1. Returnerer Grouping med korrekt Count (hvor mange af hver type doctype(s)/gruppe er der, resten ryger i Misc(hvis sat))
            SearchGroup.SortSearchGroupsAsSearchResults(groups: ref grouping, results: ref tempResults);

            var paginatedOptions = (IPaginatedExamineOptions)searchOptions;

            //2. Søge i specifikke grupper
            if (searchInGroup != null && searchInGroup.Any() && grouping.Any())
            {
                //var groupResults = new List<SearchResult>();
                var groupResults = SearchGroup.SearchInGroupsAsSearchResults(searchInGroup, paginatedOptions.Order, ref grouping, out total);

                // Apply pagination if an instance of "IPaginatedExamineOptions")
                var hest2 = paginatedOptions == null || paginatedOptions.Limit <= 0 ? groupResults : groupResults.Skip(paginatedOptions.Offset).Take(paginatedOptions.Limit);

                return hest2.Select(a => UmbracoContext.Current.ContentCache.GetById(a.Id));
            }

            grouping.ForEach(x => x.AssociatedResults.Clear());

            // Apply pagination if an instance of "IPaginatedExamineOptions")
            var hest = paginatedOptions == null || paginatedOptions.Limit <= 0 ? results : results.Skip(paginatedOptions.Offset).Take(paginatedOptions.Limit);

            // standard søgeresultat
            return hest.Select(a => UmbracoContext.Current.ContentCache.GetById(a.Id));
        }

        public static IEnumerable<SearchResult> SearchDocumentsRawResults(
            out int total,
            IExamineOptions searchOptions = null
            )
        {

            var results = SearchExamine(searchOptions, out total);

            var paginatedOptions = (IPaginatedExamineOptions)searchOptions;

            return paginatedOptions == null || paginatedOptions.Limit <= 0 ? results : results.Skip(paginatedOptions.Offset).Take(paginatedOptions.Limit);

        }

        public static IEnumerable<SearchResult> SearchDocumentsRawResults(out int total, ref List<SearchGroup> grouping, int[] groups = null, IExamineOptions searchOptions = null) {

            if (searchOptions == null) throw new ArgumentNullException(nameof(searchOptions));

            // Make the initial search in Examine
            IEnumerable<SearchResult> results = SearchExamine(searchOptions, out total, groups);

            if (grouping != null && grouping.Any()) {
                SearchGroup.SortSearchGroupsAsSearchResults(groups: ref grouping, results: ref results);
            }

            // TODO: We shouldn't assume that "searchOptions" is always an instance of "IPaginatedExamineOptions"
            IPaginatedExamineOptions paginatedOptions = (IPaginatedExamineOptions) searchOptions;

            //2. Søge i specifikke grupper
            if (groups != null && groups.Any() && grouping.Any()) {

                // Populate the groups and return the results of the selected group(s)
                List<SearchResult> groupResults = SearchGroup.SearchInGroupsAsSearchResults(groups, searchOptions.Order, ref grouping, out total);

                // Return the results
                return paginatedOptions.Limit <= 0 ? groupResults : groupResults.Skip(paginatedOptions.Offset).Take(paginatedOptions.Limit);

            }

            return paginatedOptions.Limit <= 0 ? results : results.Skip(paginatedOptions.Offset).Take(paginatedOptions.Limit);

        }

        public static IEnumerable<SearchResult> SearchExamine(ISearchOptions options, out int total) {

            // Set up the search
            BaseSearchProvider externalSearcher = ExamineManager.Instance.SearchProviderCollection[options.ExamineSearcher];
            ISearchCriteria criteria = externalSearcher.CreateSearchCriteria();
            criteria = criteria.RawQuery(options.GetRawQuery());

            ISearchResults results = externalSearcher.Search(criteria);
            total = results.TotalItemCount;

            // TODO: Handle sorting the results

            return results;

        }

        public static IOrderedEnumerable<SearchResult> SearchExamine(IExamineOptions searchOptions, out int total, int[] searchInGroup = null)
        {
            DateTime timeSpendTime = DateTime.Now;

            //search
            BaseSearchProvider externalSearcher = ExamineManager.Instance.SearchProviderCollection[searchOptions.ExamineIndex];
            ISearchCriteria criteria = externalSearcher.CreateSearchCriteria();
            criteria = criteria.RawQuery(searchOptions.GetRawQuery());

            ISearchResults tempResults = externalSearcher.Search(criteria);
            total = tempResults.TotalItemCount;
            IOrderedEnumerable<SearchResult> results;

            results = searchOptions.Order.OrderResults(externalSearcher, criteria);


            var timeSpend = DateTime.Now.Subtract(timeSpendTime).TotalSeconds.ToString();

            if (searchOptions.Debug)
            {
                LogHelper.Info<SkybrudSearch>(timeSpend + " q:" + searchOptions.GetRawQuery());
            }


            return results;
        }
    }
}