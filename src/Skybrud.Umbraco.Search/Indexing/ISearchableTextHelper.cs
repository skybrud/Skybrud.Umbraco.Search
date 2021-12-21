using System.IO;

namespace Skybrud.Umbraco.Search.Indexing {

    /// <summary>
    /// Interfaces describing a <see cref="WriteSearchableText"/> method.
    /// </summary>
    public interface ISearchableTextHelper {

        /// <summary>
        /// Writes a textual representation of this object to <paramref name="writer"/>.
        /// </summary>
        /// <param name="indexingHelper">The current <see cref="IIndexingHelper"/> instance.</param>
        /// <param name="writer">The text writer to write to.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteSearchableText(IIndexingHelper indexingHelper, TextWriter writer, string culture = null, string segment = null);

    }

}