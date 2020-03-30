namespace Skybrud.Umbraco.Search.Options.Groups {

    public class GroupedSearchOptionsBase : SearchOptionsBase, IGroupedSearchOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the groups of the search.
        /// </summary>
        public SearchGroupList Groups { get; set; }

        /// <summary>
        /// Gets or sets the method used for determining the groups each result belongs to.
        /// </summary>
        public SearchGroupMethod GroupMethod { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance with default options.
        /// </summary>
        public GroupedSearchOptionsBase() {
            Groups = new SearchGroupList();
        }

        #endregion

    }

}