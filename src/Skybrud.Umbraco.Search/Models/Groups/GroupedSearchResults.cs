using System.Collections.Generic;
using Examine;
using Skybrud.Umbraco.Search.Options;
using Skybrud.Umbraco.Search.Options.Groups;

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

            List<SearchGroupModel> groups = new List<SearchGroupModel>();

            List<SearchResult> combined = new List<SearchResult>();

            HashSet<int> hs = new HashSet<int>();

            foreach (ISearchGroup group in options.Groups) {

                SearchGroupModel searchGroupModel = new SearchGroupModel(group);
                groups.Add(searchGroupModel);

                foreach (SearchResult result in results) {
                    if (group.IsMatch(result) == false) continue;
                    searchGroupModel.Results.Add(result);
                    if (group.IsSelected && !hs.Contains(result.Id)) 
                    {
                        combined.Add(result);
                        hs.Add(result.Id);
                    } 
                }

            }

            Groups = groups.ToArray();
            Results = combined.ToArray();
            Total = Results.Length;
        }



    }

}