using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Examine;
using Skybrud.Umbraco.Search.Options.Fields;

namespace Skybrud.Umbraco.Search.Options {

    public class SearchOptionsBase : ISearchOptions {

        #region Properties

        public ISearcher Searcher { get; set; }
        
        /// <summary>
        /// Gets or sets the text to search for.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a list of fields that should be used when searching for <see cref="Fields"/>.
        /// </summary>
        public FieldList TextFields { get; set; }

        /// <summary>
        /// Gets or sets an array of IDs the returned results should be a descendant of. At least one of the IDs should
        /// be in the path of the result to be a match.
        /// </summary>
        public int[] RootIds { get; set; }

        /// <summary>
        /// Gets or sets whether the check on the <c>hideFromSearch</c> field should be disabled.
        /// </summary>
        public bool DisableHideFromSearch { get; set; }

        /// <summary>
        /// Gets whether the search should be performed in debug mode.
        /// </summary>
        public bool IsDebug { get; set; }

        #endregion

        #region Constructors

        public SearchOptionsBase() {
            Text = string.Empty;
            RootIds = null;
            TextFields = new FieldList();
        }

        #endregion

        #region Member methods

        public virtual string GetRawQuery(ISearchHelper searchHelper) {
            return string.Join(" AND ", GetQueryList(searchHelper));
        }

        protected virtual List<string> GetQueryList(ISearchHelper searchHelper) {

            List<string> query = new List<string>();

            SearchType(searchHelper, query);
            SearchText(searchHelper, query);
            SearchPath(searchHelper, query);
            SearchHideFromSearch(searchHelper, query);

            return query;

        }

        protected virtual void SearchType(ISearchHelper searchHelper, List<string> query) { }

        protected virtual void SearchText(ISearchHelper searchHelper, List<string> query) {

            if (string.IsNullOrWhiteSpace(Text)) return;

            string text = Regex.Replace(Text, @"[^\wæøåÆØÅ\-@\. ]", string.Empty).ToLowerInvariant().Trim();

            string[] terms = text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (terms.Length == 0) return;

            // fallback if no fields are added
            TextFields = TextFields ?? FieldList.GetFromStringArray(new[] { "nodeName_lci", "contentTeasertext_lci", "contentBody_lci" });
            if (TextFields.IsEmpty) return;

            query.Add(TextFields.GetQuery(terms));

        }

        protected virtual void SearchPath(ISearchHelper searchHelper, List<string> query) {
            if (RootIds == null || RootIds.Length == 0) return;
            query.Add("(" + string.Join(" OR ", from id in RootIds select "path_search:" + id) + ")");
        }

        protected virtual void SearchHideFromSearch(ISearchHelper searchHelper, List<string> query) {
            if (DisableHideFromSearch) return;
            query.Add("hideFromSearch:0");
        }

        #endregion

    }

}