namespace Skybrud.Umbraco.Search.Options.Groups {

    /// <summary>
    /// Enum class indicating how the search results should be sorted into groups.
    /// </summary>
    public enum SearchGroupMethod {

        /// <summary>
        /// Indicates that the groups of each result should only be determined through the rules of
        /// each <see cref="GroupedSearchOptionsBase.Groups"/>.
        /// </summary>
        Default,

        /// <summary>
        /// Indicates that the groups are primarily determine by the <c>groups</c> and <c>path</c>
        /// fields in Examine, then through the rules of each <see cref="GroupedSearchOptionsBase.Groups"/>.
        /// </summary>
        Examine

    }

}