using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Skybrud.Essentials.Common;
using Skybrud.Umbraco.Search.Models.Groups;
using Skybrud.Umbraco.Search.Options.Groups;
using Skybrud.Umbraco.Search.Options.Protected;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Skybrud.Umbraco.Search {

    public partial class SearchRepository {

        public virtual GroupedSearchResults SearchGroups(IGroupedSearchOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.Groups == null) throw new PropertyNotSetException(nameof(options));

            // Make the initial search in Examine
            IEnumerable<SearchResult> results = SearchExamine(options, out int total);

            if (options is IHideProtectedOptions hide && hide.HideProtected){
                results = results.Where(x => !hide.IsProtected(x));
            }

            return new GroupedSearchResults(options, results, total);

        }

    }
}