using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.Search.Composers {

    public class SearchComposer : IUserComposer {

        public void Compose(Composition composition) {

            // Hook up our main class with dependency injection
            composition.Register<ISearchHelper, SearchHelper>();

        }

    }


}