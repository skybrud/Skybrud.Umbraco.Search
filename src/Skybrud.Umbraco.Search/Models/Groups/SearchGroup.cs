using System;
using System.Web;
using Skybrud.Umbraco.Search.Options.Groups;

namespace Skybrud.Umbraco.Search.Models.Groups {

    /// <summary>
    /// Class representing a search group in a grouped search.
    /// </summary>
    public class SearchGroup {
        
        /// <summary>
        /// Gets the numeric ID of the group. The ID should primarily be used in the communication between the frontend
        /// and the backend API - eg. to indicate which groups should be shown or which groups have been selected by
        /// the user.
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Gets the friendly name of the search group. May or may not be localized depending on the implementation in
        /// the Umbraco solution.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Gets the type of the group. The type may be used to indicate the type of results that are returned in this
        /// group - eg. a site may have a one group for regular news articles and another for press releases, but if
        /// they should be rendered the same way, they could share a common value for this property.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the initial limit of this group. The limit may later be changed by the frontend.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Gets callback method used for making the search for this group.
        /// </summary>
        public Func<SearchGroup, HttpRequestBase, GroupedSearchOptionsBase, SearchGroupResultList> Callback { get; }

        /// <summary>
        /// Initializes a new search group based on the specified parameters.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="limit"></param>
        /// <param name="callback"></param>
        public SearchGroup(int id, string name, int limit, Func<SearchGroup, HttpRequestBase, GroupedSearchOptionsBase, SearchGroupResultList> callback) {
            Id = id;
            Name = name;
            Limit = limit;
            Callback = callback;
        }

        /// <summary>
        /// Initializes a new search group based on the specified parameters.
        /// </summary>
        /// <param name="id">The ID of the group.</param>
        /// <param name="name">The name of the group.</param>
        /// <param name="type">The type of the group.</param>
        /// <param name="limit">The initial limit of the group.</param>
        /// <param name="callback">The callback method of the group.</param>
        public SearchGroup(int id, string name, string type, int limit, Func<SearchGroup, HttpRequestBase, GroupedSearchOptionsBase, SearchGroupResultList> callback) {
            Id = id;
            Name = name;
            Type = type;
            Limit = limit;
            Callback = callback;
        }

    }

}