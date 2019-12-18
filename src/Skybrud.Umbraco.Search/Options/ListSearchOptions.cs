using System.Collections.Generic;
using System.Linq;
using Skybrud.Umbraco.Search.Options.Pagination;

namespace Skybrud.Umbraco.Search.Options {

    public class ListSearchOptions : SearchOptionsBase, IOffsetOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the offset to be used for pagination.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the limit to be used for pagination.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets a list of content types to be searched.
        /// </summary>
        public ContentTypeList ContentTypes { get; set; }

        #endregion

        #region Constructors

        public ListSearchOptions() {
            ContentTypes = new ContentTypeList();
        }

        #endregion

        #region Member methods

        protected override void SearchType(List<string> query) {
            if (ContentTypes == null || ContentTypes.Count == 0) return;
            query.Add($"({string.Join(" OR ", from type in ContentTypes select "__NodeTypeAlias:" + type)})");
        }

        #endregion

    }

}