using System.IO;

namespace Skybrud.Umbraco.Search.Indexing {

    /// <summary>
    /// Interfaces describing a <see cref="WriteSearchableText"/> method.
    /// </summary>
    public interface ISearchableText {

        /// <summary>
        /// Writes a textual representation of this object to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The text writer to write to.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteSearchableText(TextWriter writer, string culture = null, string segment = null);

    }

}