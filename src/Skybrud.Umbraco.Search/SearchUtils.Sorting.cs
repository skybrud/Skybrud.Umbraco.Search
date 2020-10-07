using System;
using System.Globalization;
using System.Linq;
using Examine;

namespace Skybrud.Umbraco.Search {

    public static partial class SearchUtils {

        public static class Sorting {

            public static DateTime GetSortValueByDateTime(ISearchResult result, string propertyAlias) {

                string dateString = result.GetValues(propertyAlias)?.FirstOrDefault();
                
                if (!string.IsNullOrWhiteSpace(dateString)) return DateTime.ParseExact(dateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                string createdDate = result.GetValues("createDate")?.FirstOrDefault();
                return createdDate == null ? DateTime.MinValue : new DateTime(long.Parse(createdDate));

            }

            /// <summary>
            /// Returns a <see cref="DateTime"/> parsed from the <c>contentDate</c> field, or <c>createDate</c> if nothing else is specified.
            /// </summary>
            /// <param name="result">The result.</param>
            /// <returns>An instance of <see cref="DateTime"/>.</returns>
            public static DateTime GetSortValueByContentDate(ISearchResult result) {
                return GetSortValueByDateTime(result, "contentDate");
            }

        }

    }

}