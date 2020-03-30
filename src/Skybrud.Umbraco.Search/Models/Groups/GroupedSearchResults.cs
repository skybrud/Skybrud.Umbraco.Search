using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using Examine;
using Skybrud.Essentials.Strings.Extensions;
using Skybrud.Umbraco.Search.Options;
using Skybrud.Umbraco.Search.Options.Groups;
using Umbraco.Core.Logging;
// ReSharper disable SuspiciousTypeConversion.Global

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class GroupedSearchResults {

        public int Total { get; private set; }

        public int Offset { get; }

        public int Limit { get; }

        public SearchGroupModel[] Groups { get; private set; }

        public SearchResult[] Results { get; private set; }

        public GroupedSearchResults(IGroupedSearchOptions options, IEnumerable<SearchResult> results, int total) {

            Total = total;
            ParseResults(options, results);

            if (!(options is IPaginatedSearchOptions pagination)) return;
            Offset = pagination.Offset;
            Limit = pagination.Limit;

        }

        private void ParseResults(IGroupedSearchOptions options, IEnumerable<SearchResult> results) {

            List<SearchResult> combined = new List<SearchResult>();

            if (options.GroupMethod == SearchGroupMethod.Examine) {

                Dictionary<int, SearchGroupModel> dictionary = options.Groups.Select(x => new SearchGroupModel(x)).ToDictionary(x => x.Id);

                foreach (SearchResult result in results) {

                    bool added = false;

                    foreach (int groupId in (result.Fields["groups"] ?? string.Empty).ToInt32Array()) {
                        if (dictionary.TryGetValue(groupId, out SearchGroupModel group) && group.Group.IsMatch(result)) {
                            group.Results.Add(result);
                            if (group.IsSelected && added == false) {
                                combined.Add(result);
                                added = true;
                            }
                        }
                    }

                    foreach (int nodeId in (result.Fields["path"] ?? string.Empty).ToInt32Array()) {
                        if (dictionary.TryGetValue(nodeId, out SearchGroupModel group) && group.Group.IsMatch(result)) {
                            group.Results.Add(result);
                            if (group.IsSelected && added == false) {
                                combined.Add(result);
                                added = true;
                            }
                        }
                    }

                }

                Groups = dictionary.Values.ToArray();

            } else  {

                List<SearchGroupModel> groups = new List<SearchGroupModel>();

                // Keep track of the results so each result is not added more than once
                HashSet<int> hs = new HashSet<int>();

                foreach (ISearchGroup group in options.Groups) {

                    SearchGroupModel searchGroupModel = new SearchGroupModel(group);
                    groups.Add(searchGroupModel);

                    foreach (SearchResult result in results) {
                        if (group.IsMatch(result) == false) continue;
                        searchGroupModel.Results.Add(result);
                        if (group.IsSelected && !hs.Contains(result.Id)) {
                            combined.Add(result);
                            hs.Add(result.Id);
                        }
                    }

                }

                Groups = groups.ToArray();

            }

            Results = combined.ToArray();

            Total = Results.Length;

        }



    }

}