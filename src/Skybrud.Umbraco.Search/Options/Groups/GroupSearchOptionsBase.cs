using System.Web;
using Skybrud.Essentials.Strings.Extensions;
using Skybrud.Umbraco.Search.Models.Groups;

namespace Skybrud.Umbraco.Search.Options.Groups {

    public class GroupSearchOptionsBase {

        public string Text { get; }

        public int Limit { get; }

        public int Offset { get; }

        public GroupSearchOptionsBase(SearchGroup group, HttpRequestBase request) {
            Text = request.QueryString["text"];
            Limit = request.QueryString[$"l{group.Id}"].ToInt32(group.Limit);
            Offset = request.QueryString[$"o{group.Id}"].ToInt32();
        }

    }

}