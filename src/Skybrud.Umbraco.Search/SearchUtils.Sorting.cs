using System;
using System.Globalization;
using System.Linq;
using Examine;
using Microsoft.Extensions.Logging;

namespace Skybrud.Umbraco.Search {

    public static partial class SearchUtils {

        /// <summary>
        /// Static class with various utility and helper methods related to search and sorting.
        /// </summary>
        public static class Sorting {

            ///// <summary>
            ///// Returns a <see cref="DateTime"/> value parsed from the property with specified
            ///// <paramref name="propertyAlias"/>. If the property doesn't have a value, or it's value cannot be parsed,
            ///// the <c>createDate</c> property is used instead.
            ///// </summary>
            ///// <param name="result">The search result.</param>
            ///// <param name="propertyAlias">The alias of the property holding the <see cref="DateTime"/>.</param>
            ///// <returns>The sort value as an instance of <see cref="DateTime"/>.</returns>
            //public static DateTime GetSortValueByDateTime(ISearchResult result, string propertyAlias) {
            //    return GetSortValueByDateTime(result, propertyAlias, Current.Logger);
            //}

            /// <summary>
            /// Returns a <see cref="DateTime"/> value parsed from the property with specified
            /// <paramref name="propertyAlias"/>. If the property doesn't have a value, or it's value cannot be parsed,
            /// the <c>createDate</c> property is used instead.
            /// </summary>
            /// <param name="result">The search result.</param>
            /// <param name="propertyAlias">The alias of the property holding the <see cref="DateTime"/>.</param>
            /// <param name="logger">A reference to the current logger.</param>
            /// <returns>The sort value as an instance of <see cref="DateTime"/>.</returns>
            public static DateTime GetSortValueByDateTime(ISearchResult result, string propertyAlias, ILogger logger) {

                // Get the first string value from the result
                string value = result.GetValues(propertyAlias)?.FirstOrDefault();

                // If we don't have a value, fall back to the item's creation date
                if (string.IsNullOrWhiteSpace(value)) return GetCreateDate(result);

                // Attempt to parse the value into a DateTime
                if (DateTime.TryParseExact(value, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) return dt;

                // If we reach this point, the value was not in a recognized format, so we write a warning to the log and fall back to the create date
                logger.LogWarning($"Error trying to convert to DateTime for ISearchResult with id: {result.Id} and propertyAlias: {propertyAlias}");
                return GetCreateDate(result);

            }

            ///// <summary>
            ///// Returns a <see cref="DateTime"/> value parsed from the <c>contentDate</c> field, or <c>createDate</c> if nothing else is specified.
            ///// </summary>
            ///// <param name="result">The result.</param>
            ///// <returns>An instance of <see cref="DateTime"/>.</returns>
            //public static DateTime GetSortValueByContentDate(ISearchResult result) {
            //    return GetSortValueByDateTime(result, "contentDate");
            //}

            /// <summary>
            /// Returns a <see cref="DateTime"/> value parsed from the <c>contentDate</c> field, or <c>createDate</c> if nothing else is specified.
            /// </summary>
            /// <param name="result">The result.</param>
            /// <param name="logger">A reference to the current logger.</param>
            /// <returns>An instance of <see cref="DateTime"/>.</returns>
            public static DateTime GetSortValueByContentDate(ISearchResult result, ILogger logger) {
                return GetSortValueByDateTime(result, "contentDate", logger);
            }

            /// <summary>
            /// Returns the creation date of the specified <paramref name="result"/>, or <see cref="DateTime.MinValue"/> if a creation date could not be determined.
            /// </summary>
            /// <param name="result">The result.</param>
            /// <returns>An instance of <see cref="DateTime"/> representing the creation date.</returns>
            public static DateTime GetCreateDate(ISearchResult result) {
                string createdDate = result.GetValues("createDate")?.FirstOrDefault();
                return createdDate == null ? DateTime.MinValue : new DateTime(long.Parse(createdDate));
            }

        }

    }

}