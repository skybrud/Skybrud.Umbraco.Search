using System;
using Skybrud.Essentials.Common;
using Skybrud.Umbraco.Search.Models.Groups;
using Skybrud.Umbraco.Search.Options.Groups;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Skybrud.Umbraco.Search {

    public partial class SearchRepository {

        public virtual GroupedSearchResults SearchGroups(IGroupedSearchOptions options) {

            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.Groups == null) throw new PropertyNotSetException(nameof(options));

            // Make the initial search in Examine
            var results = SearchExamine(options, out int total);

            // Wrap the results
            GroupedSearchResults result = new GroupedSearchResults(options, results, total);

            return result;

        }

    }

}