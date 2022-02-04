using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Microsoft.Extensions.Logging;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Search.Constants;
using Skybrud.Umbraco.Search.Indexing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Examine;
using Umbraco.Extensions;

namespace Skybrud.Umbraco.Search.Extensions {

    /// <summary>
    /// Static class with various extension methods for aiding indexing in Umbraco/Examine.
    /// </summary>
    public static class ExamineIndexingExtensions {

        /// <summary>
        /// Attemps to get the first string value of a field with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns><c>true</c> if the value set contains a field with the specified key; otherwise, <c>false</c>.</returns>
        public static bool TryGetString(this IndexingItemEventArgs e, string key, out string value) {
            value = e.ValueSet.Values.TryGetValue(key, out List<object> values) ? values.FirstOrDefault() as string : null;
            return value != null;
        }

        /// <summary>
        /// Adds a new field with <paramref name="key"/> and <paramref name="value"/> if the field does not already exist.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">The new value.</param>
        public static IndexingItemEventArgs AddDefaultValue(this IndexingItemEventArgs e, string key, string value) {

            // Does the field already exist?
            if (e.ValueSet.Values.ContainsKey(key)) return e;

            // Add the default value
            e.ValueSet.TryAdd(key, value);

            return e;

        }

        /// <summary>
        /// If a field with <paramref name="key"/> doesn't already exist, a new field where the key is a combination of
        /// <see cref="key"/> and <paramref name="suffix"/> will be added with <paramref name="value"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="suffix">The suffix for the key of the new field.</param>
        public static IndexingItemEventArgs AddDefaultValue(this IndexingItemEventArgs e, string key, string value, string suffix) {

            // Does the field already exist?
            if (e.ValueSet.Values.ContainsKey(key)) return e;

            // Add the default value
            e.ValueSet.TryAdd($"{key}{suffix}", value);

            return e;

        }

        /// <summary>
        /// If a field with <paramref name="key"/> exists, a new field in which commas in the value has been replaced
        /// by spaces, making each value searchable.
        ///
        /// The key of the new field will use <c>_search</c> as suffix - eg. if <paramref name="key"/> is <c>path</c>,
        /// the new field will have the key <c>path_search</c>.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the field to make searchable.</param>
        public static IndexingItemEventArgs IndexCsv(this IndexingItemEventArgs e, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return e;

            // Get the first value and replace all commas with an empty space
            string value = values.FirstOrDefault()?.ToString()?.Replace(',', ' ');

            // Added the searchable value to the index
            e.ValueSet.TryAdd($"{key}_search", value);

            return e;

        }

        /// <summary>
        /// Parses the UDIs in the field with the specified <paramref name="key"/>, and adds a new field with
        /// searchable versions of the UDIs.
        ///
        /// Specifically the method will look for any GUID based UDI's, and then format the GUIDs to formats <c>N</c>
        /// and <c>D</c> - that is <c>00000000000000000000000000000000</c> and
        /// <c>00000000-0000-0000-0000-000000000000</c>. The type of the reference entity is not added to the new field.
        ///
        /// The key of the new field will use <c>_search</c> as suffix - eg. if <paramref name="key"/> is
        /// <c>related</c>, the new field will have the key <c>related_search</c>.
        /// </summary>
        /// <param name="e">The event arguments about the node being indexed.</param>
        /// <param name="key">The key of the field to make searchable.</param>
        public static IndexingItemEventArgs IndexUdis(this IndexingItemEventArgs e, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return e;

            // Get the first value of the field
            string value = values.FirstOrDefault()?.ToString();

            // Parse the UDI's and adds as GUIDs instead (both N and D formats)
            List<string> newValues = new List<string>();
            foreach (string piece in StringUtils.ParseStringArray(value)) {
                if (UdiParser.TryParse(piece, out GuidUdi udi)) {
                    newValues.Add(udi.Guid.ToString("N"));
                    newValues.Add(udi.Guid.ToString("D"));
                } else {
                    newValues.Add(piece.Split('/').Last());
                }
            }

            // Added the searchable value to the index
            e.ValueSet.TryAdd($"{key}_search", string.Join(" ", newValues));

            return e;

        }

        /// <summary>
        /// Adds a searchable version of the date value in the field with the specified <paramref name="key"/>.
        ///
        /// The searchable value will be added in a new field using the <c>_range</c> prefix for the key (as it enables
        /// a ranged query) and the value will be formatted using <c>yyyyMMddHHmm00000</c>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        public static IndexingItemEventArgs IndexDate(this IndexingItemEventArgs e, string key) {
            return IndexDate(e, key, "yyyyMMddHHmm00000");
        }

        /// <summary>
        /// Adds searchable versions of the date values in the fields with the specified <paramref name="keys"/>.
        ///
        /// The searchable values will be added in new fields using the <c>_range</c> prefix for the keys (as it enables
        /// a ranged query) and the value will be formatted using <c>yyyyMMddHHmm00000</c>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="keys">The keys of the fields.</param>
        public static IndexingItemEventArgs IndexDate(this IndexingItemEventArgs e, params string[] keys) {
            if (keys == null) return null;
            foreach (string key in keys) IndexDate(e, key);
            return e;
        }

        /// <summary>
        /// Adds a searchable version of the date value in the field with the specified <paramref name="key"/>.
        /// 
        /// The searchable value will be added in a new field using the <c>_range</c> prefix for the key (at it enables
        /// a ranged query) and the value will be formatted using the specified <paramref name="format"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="format">The format that should be used when adding the date to the value set.</param>
        /// <param name="key">The key of the field.</param>
        public static IndexingItemEventArgs IndexDateWithFormat(this IndexingItemEventArgs e, string format, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return e;

            // Get the first value of the field
            switch (values.FirstOrDefault()) {

                case DateTime dt:

                    e.ValueSet.TryAdd($"{key}_range", dt.ToString(format));
                    break;

                    // TODO: Any other types we should handle?

            }

            return e;

        }

        /// <summary>
        /// Adds a textual representation of the block list value from property with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="logger">The logger to be used for logging errors.</param>
        /// <param name="indexingHelper">The indexing helper to be used for getting a textual representation of block list values.</param>
        /// <param name="umbracoContext">An Umbraco context used for looking up the corresponding content item of the current result.</param>
        /// <param name="key">The key (or alias) of the property holding the block list value.</param>
        /// <param name="newKey">If specified, the value of this parameter will be used for the key of the new field added to the valueset.</param>
        /// <param name="newKeySuffix">If specified, and <paramref name="newKey"/> is not also specified, the value of this parameter will be appended to <paramref name="key"/>, and used for the key of the new field added to the valueset.</param>
        public static IndexingItemEventArgs IndexBlockList(this IndexingItemEventArgs e, ILogger logger, IIndexingHelper indexingHelper, IUmbracoContext umbracoContext, string key, string newKey = null, string newKeySuffix = null) {

            // The ID is numeric, but stored as a string, so we need to parse it
            if (!int.TryParse(e.ValueSet.Id, out int id)) return e;

            // Look up the content node in the content cache
            IPublishedContent content = umbracoContext.Content.GetById(id);

            // Call the method overload to handle the rest
            return IndexBlockList(e, logger, indexingHelper, content, key, newKey, newKeySuffix);

        }

        /// <summary>
        /// Adds a textual representation of the block list value from property with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="logger">The logger to be used for logging errors.</param>
        /// <param name="indexingHelper">The indexing helper to be used for getting a textual representation of block list values.</param>
        /// <param name="content">The content item holding the block list value.</param>
        /// <param name="key">The key (or alias) of the property holding the block list value.</param>
        /// <param name="newKey">If specified, the value of this parameter will be used for the key of the new field added to the valueset.</param>
        /// <param name="newKeySuffix">If specified, and <paramref name="newKey"/> is not also specified, the value of this parameter will be appended to <paramref name="key"/>, and used for the key of the new field added to the valueset.</param>
        public static IndexingItemEventArgs IndexBlockList(this IndexingItemEventArgs e, ILogger logger, IIndexingHelper indexingHelper, IPublishedContent content, string key, string newKey = null, string newKeySuffix = null) {

            // Validate the content and the property
            if (content == null || !content.HasProperty(key)) return e;

            // Get the block list
            BlockListModel blockList = content.Value<BlockListModel>(key);
            if (blockList == null) return e;

            // Determine the new key
            newKey = newKey ?? $"{key}{newKeySuffix ?? "_search"}";

            // Get the searchable text via the indexing helper
            try {
                string text = indexingHelper.GetSearchableText(blockList);
                if (!string.IsNullOrWhiteSpace(text)) e.ValueSet.TryAdd(newKey, text);
            } catch (Exception ex) {
                logger.LogError(ex, "Failed indexing block list in property {Property} on page with ID {Id}.", key, content.Id);
            }

            return e;

        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden (excluded) from search results.
        /// </summary>
        /// <param name="e"></param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e) {
            return AddHideFromSearch(e, default(HashSet<int>));
        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden
        /// (excluded) from search results.
        ///
        /// The <paramref name="ignoreId"/> parameter can be used to specify an area of the website that should
        /// automatically be hidden from search results. This is done by checking whether the ID of the
        /// <paramref name="ignoreId"/> parameter is part of the <c>path</c> field of the valueset for the current node.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ignoreId">The ID for which the node itself and it's descendants should be hidden.</param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e, int ignoreId) {
            return AddHideFromSearch(e, new HashSet<int> { ignoreId });
        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden
        /// (excluded) from search results.
        ///
        /// The <paramref name="ignoreIds"/> parameter can be used to specify areas of the website that should
        /// automatically be hidden from search results. This is done by checking whether at least one of the IDs the
        /// <paramref name="ignoreIds"/> parameter is part of the <c>path</c> field of the valueset for the current node.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ignoreIds">The IDs for which it self and it's descendants should be hidden.</param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e, params int[] ignoreIds) {
            return AddHideFromSearch(e, new HashSet<int>(ignoreIds));
        }

        /// <summary>
        /// Adds a new <c>hideFromSearch</c> field to the valueset indicating whether the node should be hidden
        /// (excluded) from search results.
        ///
        /// The <paramref name="ignoreIds"/> parameter can be used to specify areas of the website that should
        /// automatically be hidden from search results. This is done by checking whether at least one of the IDs the
        /// <paramref name="ignoreIds"/> parameter is part of the <c>path</c> field of the valueset for the current node.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ignoreIds">The IDs for which it self and it's descendants should be hidden.</param>
        public static IndexingItemEventArgs AddHideFromSearch(this IndexingItemEventArgs e, HashSet<int> ignoreIds) {

            e.ValueSet.Values.TryGetValue(ExamineConstants.Fields.Path, out List<object> objList);
            int[] ids = StringUtils.ParseInt32Array(objList?.FirstOrDefault()?.ToString());

            if (ignoreIds != null && ids.Any(ignoreIds.Contains)) {
                e.ValueSet.Set(ExamineConstants.Fields.HideFromSearch, "1");
                return e;
            }

            if (e.ValueSet.Values.ContainsKey(ExamineConstants.Fields.HideFromSearch)) return e;

            // create empty value
            e.ValueSet.TryAdd(ExamineConstants.Fields.HideFromSearch, "0");

            return e;

        }

        /// <summary>
        /// Adds new fields with lower cased versions of the <c>nodeName</c>, <c>title</c> and <c>teaser</c> fields.
        /// </summary>
        /// <param name="e"></param>
        public static IndexingItemEventArgs AddDefaultLciFields(this IndexingItemEventArgs e) {
            return AddLciFields(e, "nodeName", "title", "teaser");
        }

        /// <summary>
        /// Adds new fields with lower cased versions of the specified <paramref name="fields"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="fields">The keys of the fields that should have a lower cased version.</param>
        public static IndexingItemEventArgs AddLciFields(this IndexingItemEventArgs e, params string[] fields) {

            // Skip non-content types
            if (e.ValueSet.Category != IndexTypes.Content) return e;

            foreach (string key in fields) {

                // Calculate the LCI key
                string lciKey = $"{key}_lci";

                // Skip if the LCI key already exists
                if (e.ValueSet.Values.ContainsKey(lciKey)) continue;

                // Get each value with "key" and add the lowwer cased versions to a new field
                foreach (object value in e.ValueSet.GetValues(key)) {
                    e.ValueSet.Add(lciKey, value.ToString()?.ToLowerInvariant());
                }

            }

            return e;

        }

    }

}