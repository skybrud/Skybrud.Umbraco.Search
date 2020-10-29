using Skybrud.Umbraco.Search.Models.Groups;

namespace Skybrud.Umbraco.Search.Options.Groups {

    public class GroupedSearchOptionsBase {

        public virtual bool TryGetLimit(SearchGroup group, out int result) {
            result = 0;
            return false;
        }

        public virtual bool TryGetOffset(SearchGroup group, out int result) {
            result = 0;
            return false;
        }

    }

}