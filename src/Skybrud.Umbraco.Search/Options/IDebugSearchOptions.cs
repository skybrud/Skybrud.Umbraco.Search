namespace Skybrud.Umbraco.Search.Options {

    /// <summary>
    /// Extends the <see cref="ISearchOptions"/> interface with a <see cref="IsDebug"/> property.
    /// </summary>
    public interface IDebugSearchOptions : ISearchOptions {

        /// <summary>
        /// Gets whether the search should be performed in debug mode.
        /// </summary>
        bool IsDebug { get; }

    }

}