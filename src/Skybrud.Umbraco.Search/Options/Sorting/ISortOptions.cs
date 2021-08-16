using Examine.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skybrud.Umbraco.Search.Options.Sorting
{
    /// <summary>
    /// Interface used for decribing how a collection of <see cref="ISearchResult"/> should be sorted during the search.
    /// </summary>
    public interface ISortOptions : ISearchOptions
    {
        /// <summary>
        /// The property field to sort after
        /// </summary>
        string SortField { get; set; }

        /// <summary>
        /// The sort type of the property field
        /// </summary>
        /// <remarks>
        /// For FullTextSortable use <see cref="SortType.String"/> and for DateTime use <see cref="SortType.Long"/>
        /// </remarks>
        SortType SortType { get; set; }

        /// <summary>
        /// If true the result are sorted in acending order. If false the result is sorted in decending order.
        /// </summary>
        bool SortAcending { get; set; }
    }
}
