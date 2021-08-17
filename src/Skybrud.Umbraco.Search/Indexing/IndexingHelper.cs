using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json;
using Skybrud.Essentials.Strings;
using Umbraco.Core;
using Umbraco.Core.Models.Blocks;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

namespace Skybrud.Umbraco.Search.Indexing {
 
    public class IndexingHelper {

        /// <summary>
        /// Returns a textual representation of the specified published <paramref name="element"/>.
        /// </summary>
        /// <param name="element">An instance of <see cref="IPublishedElement"/>.</param>
        /// <returns>The textual representation</returns>
        public virtual string GetSearchableText(IPublishedElement element) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteElement(writer, element);
            return sb.ToString();
        }

        /// <summary>
        /// Returns a textual representation of the specified <paramref name="blockList"/>.
        /// </summary>
        /// <param name="blockList">An instance of <see cref="BlockListModel"/>.</param>
        /// <returns>The textual representation</returns>
        public virtual string GetSearchableText(BlockListModel blockList) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteBlockList(writer, blockList);
            return sb.ToString();
        }
        
        /// <summary>
        /// Returns a textual representation of the specified <paramref name="token"/>.
        /// </summary>
        /// <param name="token">An instance of <see cref="JToken"/>.</param>
        /// <returns>The textual representation</returns>
        public virtual string GetSearchableText(JToken token) {
            StringBuilder sb = new StringBuilder();
            using (TextWriter writer = new StringWriter(sb)) WriteJsonToken(writer, token);
            return sb.ToString();
        }

        /// <summary>
        /// Appends a textual representation of the specified <paramref name="value"/> to <paramref name="writer"/>.
        ///
        /// Values that appear to be containing one or more UDIs will be ignored. Values that appear to be JSON will be parsed 
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value.</param>
        public virtual void WriteString(TextWriter writer, string value) {

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
            writer.WriteLine(StringUtils.StripHtml(value));

        }

        /// <summary>
        /// Appends a textual representation of the value of <paramref name="property"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="owner">The parent <see cref="IPublishedElement"/> of <paramref name="property"/>.</param>
        /// <param name="property">The property.</param>
        public virtual void WriteProperty(TextWriter writer, IPublishedElement owner, IPublishedProperty property) {
            WriteValue(writer, property.Value());
        }
        
        /// <summary>
        /// Appends a textual representation of the specified published <paramref name="element"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="element">The instance of <see cref="IPublishedElement"/> to append.</param>
        public virtual void WriteElement(TextWriter writer, IPublishedElement element) {

            if (element == null) return;

            if (element is IPublishedContent content) {
                writer.WriteLine(content.Name);
            }

            foreach (IPublishedProperty property in element.Properties) {

                WriteProperty(writer, element, property);

            }

        }
        
        /// <summary>
        /// Appends a textual representation of the specified <paramref name="blockList"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="blockList">The instance of <see cref="BlockListModel"/> to append.</param>
        public virtual void WriteBlockList(TextWriter writer, BlockListModel blockList) {

            if (blockList == null) return;

            foreach (BlockListItem block in blockList) {
                WriteElement(writer, block.Content);
            }

        }
        
        /// <summary>
        /// Appends a textual representation of the specified JSON <paramref name="token"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="token">The instance of <see cref="JToken"/> to append.</param>
        public virtual void WriteJsonToken(TextWriter writer, JToken token) {

            // Check the type of "token" to detect null values, objects and arrays
            switch (token) {

                case null:
                    return;
                
                case JObject obj:
                    WriteJsonObject(writer, obj);
                    return;

                case JArray array:
                    WriteJsonArray(writer, array);
                    return;

            }

            // For other types, check the "Type" property instead
            switch (token.Type) {
                
                case JTokenType.String:
                    WriteString(writer, token.Value<string>());
                    return;

            }

        }
        
        /// <summary>
        /// Appends a textual representation of the specified <paramref name="json"/> object to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="json">The instance of <see cref="JObject"/> to append.</param>
        protected virtual void WriteJsonObject(TextWriter writer, JObject json) {

            foreach (JProperty prop in json.Properties()) {

                switch (prop.Name) {

                    // Skip known BlockList properties that doesn't contain relevant text
                    case "Umbraco.BlockList":
                    case "contentTypeKey":
                        continue;

                    // For all other properties, run through normal index logic
                    default:
                        WriteJsonToken(writer, prop.Value);
                        break;

                }

            }

        }
        
        /// <summary>
        /// Appends a textual representation of the specified JSON <paramref name="array"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="array">The instance of <see cref="JArray"/> to append.</param>
        protected virtual void WriteJsonArray(TextWriter writer, JArray array) {
            if (array == null) return;
            foreach (JToken item in array) WriteJsonToken(writer, item);
        }
        
        /// <summary>
        /// Appends a textual representation of the specified <paramref name="value"/> to <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value.</param>
        public virtual void WriteValue(TextWriter writer, object value) {

            switch (value) {

                case string str:
                    WriteString(writer, str);
                    break;

                case JToken json:
                    WriteJsonToken(writer, json);
                    break;

                case IHtmlString html:
                    writer.WriteLine(StringUtils.StripHtml(html.ToString()));
                    break;

                case BlockListModel blockList:
                    WriteBlockList(writer, blockList);
                    break;

                case IPublishedElement element:
                    WriteElement(writer, element);
                    break;

                case IEnumerable<IPublishedElement> collection:
                    foreach (var element in collection) WriteElement(writer, element);
                    break;

            }

        }

    }

}