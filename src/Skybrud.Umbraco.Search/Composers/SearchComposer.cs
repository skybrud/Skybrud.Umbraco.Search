using Umbraco.Core;
using Umbraco.Core.Composing;

namespace Skybrud.Umbraco.Search.Composers {

    public class MyComposer : IUserComposer {

        public void Compose(Composition composition) {

            // Hook up our main class with dependency injection
            composition.Register<SkybrudSearch, SkybrudSearch>();

        }

    }


}