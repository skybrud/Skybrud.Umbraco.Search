using Examine;

namespace Skybrud.Umbraco.Search.Options.Groups {

    public interface IGroupIsMatch {

        bool IsMatch(ISearchGroup group, SearchResult result);

    }

}