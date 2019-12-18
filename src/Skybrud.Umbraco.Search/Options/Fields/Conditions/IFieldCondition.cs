using Examine;

namespace Skybrud.Umbraco.Search.Options.Fields.Conditions {

    public interface IFieldCondition {

        /// <summary>
        /// Gets the alias of the field to match.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Returns whether the specified <paramref name="result"/> matches this condition.
        /// </summary>
        /// <param name="result">The result to check.</param>
        /// <returns><c>true</c> if the result matches this condition, otherwise <c>false</c>.</returns>
        bool IsMatch(SearchResult result);

    }

}