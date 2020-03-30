namespace Skybrud.Umbraco.Search.Options.Groups {

    public class GroupedSearchOptionsBase : SearchOptionsBase, IGroupedSearchOptions {

        #region Properties

        /// <summary>
        /// Gets or sets the groups of the search.
        /// </summary>
        public SearchGroupList Groups { get; set; }
        
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