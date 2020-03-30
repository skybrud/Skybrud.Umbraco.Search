using System.Collections.Generic;
using Examine;
using Examine.SearchCriteria;

namespace Skybrud.Umbraco.Search.Options.Sorting  {

    public interface ISortOptions : ISearchOptions {

        IEnumerable<SearchResult> Sort(IEnumerable<SearchResult> results);

    }

    public interface IExamineSearchCriteriaOptions : ISearchOptions {

        void UpdateSearchCriteria(ISearchCriteria criteria);

    }

}