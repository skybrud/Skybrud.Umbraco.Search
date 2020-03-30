namespace Skybrud.Umbraco.Search.Options  {

    public interface ISearchOptions {

        /// <summary>
        /// Gets or sets the alias of the alias of the Examine searcher to be used.
        /// </summary>
        string ExamineSearcher { get; set; }

        /// <summary>
        /// Gets the raw query for the search.
        /// </summary>
        /// <returns></returns>
        string GetRawQuery();

    }
}