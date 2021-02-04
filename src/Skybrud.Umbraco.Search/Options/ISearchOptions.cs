namespace Skybrud.Umbraco.Search.Options  {

    public interface ISearchOptions {

        /// <summary>
        /// Gets the raw query for the search.
        /// </summary>
        /// <returns></returns>
        string GetRawQuery(ISearchHelper searchHelper);

    }
}