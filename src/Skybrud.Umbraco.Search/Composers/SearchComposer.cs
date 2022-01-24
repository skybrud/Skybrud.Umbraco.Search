using Microsoft.Extensions.DependencyInjection;
using Skybrud.Umbraco.Search.Indexing;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Skybrud.Umbraco.Search.Composers {

    public class SearchComposer : IComposer {

        public void Compose(IUmbracoBuilder builder) {
            builder.Services.AddTransient<ISearchHelper, SearchHelper>();
            builder.Services.AddTransient<IIndexingHelper, IndexingHelper>();
        }
    }

}