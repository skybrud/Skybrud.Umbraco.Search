using Skybrud.Umbraco.Search.Indexing;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.Search.Composers {

    [RuntimeLevel(MinLevel = RuntimeLevel.Boot)]
    public class SearchComposer : IUserComposer {

        public void Compose(Composition composition) {

            // Set up dependency injection
            composition.Register<ISearchHelper, SearchHelper>();
            composition.Register<IIndexingHelper, IndexingHelper>();

        }

    }

}