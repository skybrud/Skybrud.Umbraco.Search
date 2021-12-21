using Microsoft.AspNetCore.Html;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Skybrud.Umbraco.Search.Indexing {

    /// <summary>
    /// Helper class to aid in various indexing tasks.
    /// </summary>
    public class IndexingHelper : IIndexingHelper {

        /// <inheritdoc />
        public virtual string GetSearchableText(object value, string culture = null, string segment = null) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteValue(writer, value, culture, segment);
            return sb.ToString();
        }

        /// <inheritdoc />
        public virtual string GetSearchableText(IPublishedElement element, string culture = null, string segment = null) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteElement(writer, element, culture, segment);
            return sb.ToString();
        }

        /// <inheritdoc />
        public virtual string GetSearchableText(BlockListModel blockList, string culture = null, string segment = null) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteBlockList(writer, blockList, culture, segment);
            return sb.ToString();
        }

        /// <inheritdoc />
        public virtual string GetSearchableText(BlockListItem blockListItem, string culture = null, string segment = null) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteBlockListItem(writer, blockListItem, culture, segment);
            return sb.ToString();
        }

        /// <inheritdoc />
        public virtual string GetSearchableText(JToken token, string culture = null, string segment = null) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteJsonToken(writer, token, culture, segment);
            return sb.ToString();
        }

        /// <inheritdoc />
        public virtual void WriteString(TextWriter writer, string value, string culture = null, string segment = null) {

            // Nothing to index if the value is empty
            if (string.IsNullOrEmpty(value)) return;

            // Ignore UDIs
            if (value.StartsWith("umb://")) return;

            // If the string contains a JSON blob, we parse and get the searchable text for that blob as well
            if (value.DetectIsJson()) {
                WriteJsonToken(writer, JsonUtils.ParseJsonToken(value));
                return;
            }

            // Strip the HTML from "value" and append it to the writer
            writer.WriteLine(StripHtml(value));

        }

        /// <inheritdoc />
        public virtual void WriteProperty(TextWriter writer, IPublishedElement owner, IPublishedProperty property, string culture = null, string segment = null) {
            WriteValue(writer, property.Value(null, culture, segment));
        }

        /// <inheritdoc />
        public virtual void WriteElement(TextWriter writer, IPublishedElement element, string culture = null, string segment = null) {

            switch (element) {

                case null:
                    return;

                case ISearchableText st:
                    st.WriteSearchableText(writer, culture, segment);
                    return;

                case ISearchableTextHelper sth:
                    sth.WriteSearchableText(this, writer, culture, segment);
                    return;

                case IPublishedContent content:
                    writer.WriteLine(content.Name(culture));
                    break;

            }

            foreach (IPublishedProperty property in element.Properties) {

                WriteProperty(writer, element, property, culture, segment);

            }

        }

        /// <inheritdoc />
        public virtual void WriteBlockList(TextWriter writer, BlockListModel blockList, string culture = null, string segment = null) {
            if (blockList == null) return;
            foreach (BlockListItem block in blockList) {
                WriteBlockListItem(writer, block, culture, segment);
            }
        }

        /// <inheritdoc />
        public virtual void WriteBlockListItem(TextWriter writer, BlockListItem blockListItem, string culture = null, string segment = null) {
            if (blockListItem == null) return;
            WriteElement(writer, blockListItem.Content, culture, segment);
        }

        /// <inheritdoc />
        public virtual void WriteJsonToken(TextWriter writer, JToken token, string culture = null, string segment = null) {

            // Check the type of "token" to detect null values, objects and arrays
            switch (token) {

                case null:
                    return;

                case JObject obj:
                    WriteJsonObject(writer, obj, culture, segment);
                    return;

                case JArray array:
                    WriteJsonArray(writer, array, culture, segment);
                    return;

            }

            // For other types, check the "Type" property instead
            switch (token.Type) {

                case JTokenType.String:
                    WriteString(writer, token.Value<string>(), culture, segment);
                    return;

            }

        }

        /// <inheritdoc />
        public virtual void WriteJsonObject(TextWriter writer, JObject json, string culture = null, string segment = null) {

            foreach (JProperty prop in json.Properties()) {

                switch (prop.Name) {

                    // Skip known BlockList properties that doesn't contain relevant text
                    case "Umbraco.BlockList":
                    case "contentTypeKey":
                        continue;

                    // For all other properties, run through normal index logic
                    default:
                        WriteJsonToken(writer, prop.Value, culture, segment);
                        break;

                }

            }

        }

        /// <inheritdoc />
        public virtual void WriteJsonArray(TextWriter writer, JArray array, string culture = null, string segment = null) {
            if (array == null) return;
            foreach (JToken item in array) WriteJsonToken(writer, item, culture, segment);
        }

        /// <inheritdoc />
        public virtual void WriteValue(TextWriter writer, object value, string culture = null, string segment = null) {

            switch (value) {

                case ISearchableText st:
                    st.WriteSearchableText(writer, culture, segment);
                    break;

                case ISearchableTextHelper sth:
                    sth.WriteSearchableText(this, writer, culture, segment);
                    break;

                case string str:
                    WriteString(writer, str, culture, segment);
                    break;

                case JToken json:
                    WriteJsonToken(writer, json, culture, segment);
                    break;

                case HtmlString html:
                    writer.WriteLine(StripHtml(html.ToString()));
                    break;

                case BlockListModel blockList:
                    WriteBlockList(writer, blockList, culture, segment);
                    break;

                case IPublishedElement element:
                    WriteElement(writer, element, culture, segment);
                    break;

                case IEnumerable<IPublishedElement> collection:
                    foreach (IPublishedElement element in collection) WriteElement(writer, element, culture, segment);
                    break;

            }

        }

        /// <summary>
        /// Strips the HTML from the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The HTML value to be stripped.</param>
        /// <returns>The value stripped for HTML.</returns>
        protected virtual string StripHtml(string value) {
            return HttpUtility.HtmlDecode(Regex.Replace(value, "<.*?>", " "));
        }

    }

}