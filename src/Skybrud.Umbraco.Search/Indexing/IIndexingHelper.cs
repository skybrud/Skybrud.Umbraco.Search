using System.IO;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Skybrud.Umbraco.Search.Indexing {

    /// <summary>
    /// Interface describin a helper class to aid in various indexing tasks.
    /// </summary>
    public interface IIndexingHelper {

        /// <summary>
        /// Returns a textual representation of the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        /// <returns>The textual representation</returns>
        string GetSearchableText(object value, string culture = null, string segment = null);

        /// <summary>
        /// Returns a textual representation of the specified published <paramref name="element"/>.
        /// </summary>
        /// <param name="element">An instance of <see cref="IPublishedElement"/>.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        /// <returns>The textual representation</returns>
        string GetSearchableText(IPublishedElement element, string culture = null, string segment = null);

        /// <summary>
        /// Returns a textual representation of the specified <paramref name="blockList"/>.
        /// </summary>
        /// <param name="blockList">An instance of <see cref="BlockListModel"/>.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        /// <returns>The textual representation</returns>
        string GetSearchableText(BlockListModel blockList, string culture = null, string segment = null);

        /// <summary>
        /// Returns a textual representation of the specified <paramref name="blockListItem"/>.
        /// </summary>
        /// <param name="blockListItem">An instance of <see cref="BlockListItem"/>.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        /// <returns>The textual representation</returns>
        string GetSearchableText(BlockListItem blockListItem, string culture = null, string segment = null);

        /// <summary>
        /// Returns a textual representation of the specified <paramref name="token"/>.
        /// </summary>
        /// <param name="token">An instance of <see cref="JToken"/>.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        /// <returns>The textual representation</returns>
        string GetSearchableText(JToken token, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified <paramref name="value"/> to <paramref name="writer"/>.
        ///
        /// Values that appear to be containing one or more UDIs will be ignored. Values that appear to be JSON will be parsed 
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteString(TextWriter writer, string value, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the value of <paramref name="property"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="owner">The parent <see cref="IPublishedElement"/> of <paramref name="property"/>.</param>
        /// <param name="property">The property.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteProperty(TextWriter writer, IPublishedElement owner, IPublishedProperty property, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified published <paramref name="element"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="element">The instance of <see cref="IPublishedElement"/> to append.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteElement(TextWriter writer, IPublishedElement element, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified <paramref name="blockList"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="blockList">The instance of <see cref="BlockListModel"/> to append.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteBlockList(TextWriter writer, BlockListModel blockList, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified <paramref name="blockListItem"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="blockListItem">The instance of <see cref="BlockListModel"/> to append.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteBlockListItem(TextWriter writer, BlockListItem blockListItem, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified JSON <paramref name="token"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="token">The instance of <see cref="JToken"/> to append.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteJsonToken(TextWriter writer, JToken token, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified <paramref name="json"/> object to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="json">The instance of <see cref="JObject"/> to append.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteJsonObject(TextWriter writer, JObject json, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified JSON <paramref name="array"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="array">The instance of <see cref="JArray"/> to append.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteJsonArray(TextWriter writer, JArray array, string culture = null, string segment = null);

        /// <summary>
        /// Appends a textual representation of the specified <paramref name="value"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="culture">The variation language.</param>
        /// <param name="segment">The variation segment.</param>
        void WriteValue(TextWriter writer, object value, string culture = null, string segment = null);

    }

}