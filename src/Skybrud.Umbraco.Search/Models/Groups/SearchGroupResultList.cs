using Newtonsoft.Json;
using System.Collections.Generic;

namespace Skybrud.Umbraco.Search.Models.Groups {

    /// <summary>
    /// Class representing a post search version of <see cref="SearchGroup"/>, including the results returned by the
    /// search for this particular group.
    /// </summary>
    public class SearchGroupResultList {

        /// <summary>
        /// Gets the numeric ID of the group. The ID should primarily be used in the communication between the frontend
        /// and the backend API - eg. to indicate which groups should be shown or which groups have been selected by
        /// the user.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; }

        /// <summary>
        /// Gets the friendly name of the search group. May or may not be localized depending on the implementation in
        /// the Umbraco solution.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; }

        /// <summary>
        /// Gets the type of the group. The type may be used to indicate the type of results that are returned in this
        /// group - eg. a site may have a one group for regular news articles and another for press releases, but if
        /// they should be rendered the same way, they could share a common value for this property.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; }

        /// <summary>
        /// Gets the total amount of results within this group.
        /// </summary>
        [JsonProperty("total")]
        public long Total { get; }

        /// <summary>
        /// Gets the current limit of this group.
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; }

        /// <summary>
        /// Gets the current offset of this group.
        /// </summary>
        [JsonProperty("offset")]
        public long Offset { get; }

        /// <summary>
        /// Gets the items/results for this group. When using pagination, this property will only contain the result of
        /// the current page.
        /// </summary>
        [JsonProperty("items")]
        public IEnumerable<object> Items { get; }

        /// <summary>
        /// Initializes a new instance based on the specified <paramref name="group"/>.
        /// </summary>
        /// <param name="group">The <see cref="SearchGroup"/> this instance should be based on.</param>
        /// <param name="limit">The current limit of this group.</param>
        /// <param name="offset">The current offset of this group.</param>
        /// <param name="total">The total amount of results within this group.</param>
        /// <param name="items">The items/results for this group.</param>
        public SearchGroupResultList(SearchGroup group, int limit, long offset, long total, IEnumerable<object> items) {
            Id = group.Id;
            Name = group.Name;
            Type = group.Type;
            Total = total;
            Limit = limit;
            Offset = offset;
            Items = items;
        }

    }

}