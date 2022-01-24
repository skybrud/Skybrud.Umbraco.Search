using Microsoft.AspNetCore.Http;
using Skybrud.Umbraco.Search.Models.Groups;
using System;

namespace Skybrud.Umbraco.Search.Options.Groups {

    public class GroupSearchOptionsBase {

        public string Text { get; }

        public int Limit { get; }

        public int Offset { get; }

        public GroupSearchOptionsBase(SearchGroup group, HttpRequest request) {
            Limit = group.Limit;
            if (request.Query.TryGetValue("text", out var textRawValue)) Text = textRawValue.ToString();
            if (request.Query.TryGetValue($"l{group.Id}", out var limitRawValue)) {
                Limit = Convert.ToInt32(limitRawValue);
            }
            if (request.Query.TryGetValue($"o{group.Id}", out var offsetRawValue)) {
                Offset = Convert.ToInt32(offsetRawValue);
            }
        }

    }

}