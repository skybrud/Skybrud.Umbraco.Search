using Microsoft.AspNetCore.Http;
using Skybrud.Essentials.AspNetCore;
using Skybrud.Umbraco.Search.Models.Groups;

namespace Skybrud.Umbraco.Search.Options.Groups {

    public class GroupSearchOptionsBase {

        public string Text { get; }

        public int Limit { get; }

        public int Offset { get; }

        public GroupSearchOptionsBase(SearchGroup group, HttpRequest request) {
            Text = request.Query.GetString("text");
            Limit = request.Query.GetInt32($"l{group.Id}", group.Limit);
            Offset = request.Query.GetInt32($"o{group.Id}");
        }

    }

}