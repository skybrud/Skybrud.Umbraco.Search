using Examine;
using System;
using System.Globalization;
using System.Linq;

namespace Skybrud.Umbraco.Search {

    public static partial class SearchUtils {

        /// <summary>
        /// Static class with various utility and helper methods related to search and sorting.
        /// </summary>
        public static class Sorting {

            /// <summary>
            /// Returns a <see cref="DateTime"/> value parsed from the property with specified <paramref name="propertyAlias"/>. If the property doesn't have a value, or it's value cannot be parsed, the <c>createDate</c> property is used instead.
            /// </summary>
            /// <param name="result">The search result.</param>
            /// <param name="propertyAlias"></param>
            /// <returns>The sort value as an instance of <see cref="DateTime"/>.</returns>
            public static DateTime GetSortValueByDateTime(ISearchResult result, string propertyAlias) {

                string dateString = result.GetValues(propertyAlias)?.FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(dateString)) return DateTime.ParseExact(dateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                string createdDate = result.GetValues("createDate")?.FirstOrDefault();
                return createdDate == null ? DateTime.MinValue : new DateTime(long.Parse(createdDate));

            }

            /// <summary>
            /// Returns a <see cref="DateTime"/> value parsed from the <c>contentDate</c> field, or <c>createDate</c> if nothing else is specified.
            /// </summary>
            /// <param name="result">The result.</param>
            /// <returns>An instance of <see cref="DateTime"/>.</returns>
            public static DateTime GetSortValueByContentDate(ISearchResult result) {
                return GetSortValueByDateTime(result, "contentDate");
            }

        }

    }

}