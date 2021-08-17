using Examine.Search;
using Examine;
using Skybrud.Essentials.Collections;

namespace Skybrud.Umbraco.Search.Options.Sorting {
    
    /// <summary>
    /// Interface used for decribing how a collection of <see cref="ISearchResult"/> should be sorted during the search.
    /// </summary>
    public interface ISortOptions : ISearchOptions {
        
        /// <summary>
        /// The property field to sort after.
        /// </summary>
        string SortField { get; set; }

        /// <summary>
        /// The sort type of the property field.
        /// </summary>
        /// <remarks>
        /// For FullTextSortable use <see cref="SortType.String"/> and for DateTime use <see cref="SortType.Long"/>.
        /// </remarks>
        SortType SortType { get; set; }

        /// <summary>
        /// Gets or sets the order by which the results should be sorted. Possible values are <see cref="SortOrder.Ascending"/> and <see cref="Essentials.Collections.SortOrder.Descending"/>.
        /// </summary>
        SortOrder SortOrder { get; set; }

    }

}