using Examine;
using Examine.Search;
using Skybrud.Umbraco.Search.Options.Text;

namespace Skybrud.Umbraco.Search.Options {

    public class TextQuerySearchOptionsBase : ISearchOptions {

        #region Properties

        public ISearcher Searcher { get; set; }

        public bool IsDebug { get; set; }

        public ITextQuery Text { get; set; }

        #endregion

        #region Member methods

        public virtual IBooleanOperation GetBooleanOperation(ISearchHelper searchHelper, ISearcher searcher, IQuery query) {
            return query.NativeQuery(string.Join(" AND ", GetQueryList(searchHelper)));
        }

        protected virtual QueryList GetQueryList(ISearchHelper searchHelper) {

            QueryList query = new QueryList();

            SearchType(searchHelper, query);
            SearchText(searchHelper, query);
            //SearchPath(searchHelper, query);
            //SearchHideFromSearch(searchHelper, query);

            return query;

        }

        protected virtual void SearchType(ISearchHelper searchHelper, QueryList query) { }

        protected virtual void SearchText(ISearchHelper searchHelper, QueryList query) {

            if (Text == null) return;

            string textQuery = Text.GetRawQuery();

            if (string.IsNullOrWhiteSpace(textQuery)) return;

            query.Add(textQuery);

        }

        #endregion

    }

}