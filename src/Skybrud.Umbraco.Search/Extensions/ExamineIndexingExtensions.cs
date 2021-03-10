using System.Collections.Generic;
using System.Linq;
using Examine;
using Skybrud.Essentials.Strings;
using Umbraco.Core;

namespace Skybrud.Umbraco.Search.Extensions {

    /// <summary>
    /// Static class with various extension methods for aiding indexing in Umbraco/Examine.
    /// </summary>
    public static class ExamineIndexingExtensions {

        /// <summary>
        /// If a field with <paramref name="key"/> exists, a new field in which commas in the value has been replaced
        /// by spaces, making each value searchable.
        ///
        /// The key of the new field will use <c>_search</c> as suffix - eg. if <paramref name="key"/> is <c>path</c>,
        /// the new field will have the key <c>path_search</c>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key"></param>
        public static void MakeCsvSearchable(this IndexingItemEventArgs e, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return;

            // Get the first value and replace all commas with an empty space
            string value = values.FirstOrDefault()?.ToString().Replace(',', ' ');

            // Added the searchable value to the index
            e.ValueSet.TryAdd($"{key}_search", value);

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
        /// <param name="e"></param>
        /// <param name="key">The key of the field to make searchable.</param>
        public static void MakeUdisSearchable(this IndexingItemEventArgs e, string key) {

            // Attempt to get the values of the specified field
            if (!e.ValueSet.Values.TryGetValue(key, out List<object> values)) return;

            // Get the first value of the field
            string value = values.FirstOrDefault()?.ToString();

            // Parse the UDI's and adds as GUIDs instead (both N and D formats)
            List<string> newValues = new List<string>();
            foreach (string piece in StringUtils.ParseStringArray(value)) {
                if (GuidUdi.TryParse(piece, out GuidUdi udi)) {
                    newValues.Add(udi.Guid.ToString("N"));
                    newValues.Add(udi.Guid.ToString("D"));
                } else {
                    newValues.Add(piece.Split('/').Last());
                }
            }

            // Added the searchable value to the index
            e.ValueSet.TryAdd($"{key}_search", string.Join(" ", newValues));

        }

        /// <summary>
        /// Adds a new field with <paramref name="key"/> and <paramref name="value"/> if the field does not already exist.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">The new value.</param>
        public static void AddDefaultValue(this IndexingItemEventArgs e, string key, string value) {

            // Does the field already exist?
            if (e.ValueSet.Values.ContainsKey(key)) return;

            // Add the default value
            e.ValueSet.TryAdd(key, value);

        }

        /// <summary>
        /// If a field with <paramref name="key"/> doesn't already exist, a new field where the key is a combination of
        /// <see cref="key"/> and <paramref name="suffix"/> will be added with <paramref name="value"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="key">The key of the field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="suffix">The suffix for the key of the new field.</param>
        public static void AddDefaultValue(this IndexingItemEventArgs e, string key, string value, string suffix) {

            // Does the field already exist?
            if (e.ValueSet.Values.ContainsKey(key)) return;

            // Add the default value
            e.ValueSet.TryAdd($"{key}{suffix}", value);

        }

    }

}