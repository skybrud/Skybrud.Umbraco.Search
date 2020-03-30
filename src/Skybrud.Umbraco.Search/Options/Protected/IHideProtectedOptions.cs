using Examine;

namespace Skybrud.Umbraco.Search.Options.Protected {

    public interface IHideProtectedOptions {

        bool HideProtected { get; }

        bool IsProtected(SearchResult result);

    }

}