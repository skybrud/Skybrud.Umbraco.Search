namespace Skybrud.Umbraco.Search.Options {
    
    /// <summary>
    /// Enum class indicating the type of a <see cref="QueryList"/>.
    /// </summary>
    public enum QueryListType {

        /// <summary>
        /// Indicates that a <see cref="QueryList"/> should be <strong>AND</strong>-based.
        /// </summary>
        And,
        
        /// <summary>
        /// Indicates that a <see cref="QueryList"/> should be <strong>OR</strong>-based.
        /// </summary>
        Or

    }

}