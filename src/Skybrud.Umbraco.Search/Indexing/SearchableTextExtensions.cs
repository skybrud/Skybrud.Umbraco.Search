using System.IO;
using System.Text;

namespace Skybrud.Umbraco.Search.Indexing {

    /// <summary>
    /// Static class with extension methods for <see cref="ISearchableText"/>.
    /// </summary>
    public static class SearchableTextExtensions {

        /// <summary>
        /// Returns the textual representation of the specified <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to convert to text.</param>
        /// <returns>The textual representation.</returns>
        public static string GetSearchableText(this ISearchableText obj) {
            if (obj == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) obj.WriteSearchableText(writer);
            return sb.ToString();
        }

        /// <summary>
        /// Returns the textual representation of the specified <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj">The object to convert to text.</param>
        /// <param name="indexingHelper">The current <see cref="IIndexingHelper"/> instance.</param>
        /// <returns>The textual representation.</returns>
        public static string GetSearchableText(this ISearchableTextHelper obj, IIndexingHelper indexingHelper) {
            if (obj == null) return string.Empty;
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) obj.WriteSearchableText(indexingHelper, writer);
            return sb.ToString();
        }

    }

}