using System.Collections.Generic;
using Examine;
using Skybrud.Umbraco.Search.Options.Text;

namespace Skybrud.Umbraco.Search.Options {

    public class TextQuerySearchOptionsBase : ISearchOptions {

        #region Properties

        public ISearcher Searcher { get; set; }

        public bool IsDebug { get; set; }

        public ITextQuery Text { get; set; }

        #endregion

        #region Member methods

        public virtual string GetRawQuery(ISearchHelper searchHelper) {
            return string.Join(" AND ", GetQueryList(searchHelper));
        }

        protected virtual List<string> GetQueryList(ISearchHelper searchHelper) {

            List<string> query = new List<string>();

            SearchType(searchHelper, query);
            SearchText(searchHelper, query);
            //SearchPath(searchHelper, query);
            //SearchHideFromSearch(searchHelper, query);

            return query;

        }

        protected virtual void SearchType(ISearchHelper searchHelper, List<string> query) { }

        protected virtual void SearchText(ISearchHelper searchHelper, List<string> query) {

            if (Text == null) return;

            string textQuery = Text.GetRawQuery();

            if (string.IsNullOrWhiteSpace(textQuery)) return;

            query.Add(textQuery);

        }

        #endregion

    }
}