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

        public virtual string GetRawQuery() {
            return string.Join(" AND ", GetQueryList());
        }

        protected virtual List<string> GetQueryList() {

            List<string> query = new List<string>();

            SearchType(query);
            SearchText(query);
            //SearchPath(query);
            //SearchHideFromSearch(query);

            return query;

        }

        protected virtual void SearchType(List<string> query) { }

        protected virtual void SearchText(List<string> query) {

            if (Text == null) return;

            string textQuery = Text.GetRawQuery();

            if (string.IsNullOrWhiteSpace(textQuery)) return;

            query.Add(textQuery);

        }

        #endregion

    }
}