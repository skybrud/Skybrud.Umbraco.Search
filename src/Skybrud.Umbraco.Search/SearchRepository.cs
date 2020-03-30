using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Providers;
using Examine.SearchCriteria;
using Skybrud.Umbraco.Search.Options;

namespace Skybrud.Umbraco.Search {

    public partial class SearchRepository {

        public virtual IEnumerable<SearchResult> Search(ISearchOptions options) {
            return Search(options, out _);
        }

        public virtual IEnumerable<SearchResult> Search(ISearchOptions options, out int total) {
            
            if (options == null) throw new ArgumentNullException(nameof(options));

            // Make the initial search in Examine
            IEnumerable<SearchResult> results = SearchExamine(options, out total);

            // Apply pagination
            if (options is IPaginatedSearchOptions pagination && pagination.Limit > 0) {
                return results.Skip(pagination.Offset).Take(pagination.Limit);
            }
            
            return results;

        }
        
        /// <summary>
        /// Creates a raw search in Examine based on the specified <paramref name="options"/>.
        /// </summary>
        /// <param name="options">The options for the search.</param>
        /// <param name="total">The total amount of results matching <paramref name="options"/>.</param>
        /// <returns></returns>
        public virtual IEnumerable<SearchResult> SearchExamine(ISearchOptions options, out int total) {

            // Set up the search
            BaseSearchProvider externalSearcher = ExamineManager.Instance.SearchProviderCollection[options.ExamineSearcher];
            ISearchCriteria criteria = externalSearcher.CreateSearchCriteria();
            criteria = criteria.RawQuery(options.GetRawQuery());

            ISearchResults results = externalSearcher.Search(criteria);
            total = results.TotalItemCount;

            // TODO: Handle sorting the results

            return results;

        }

    }

}