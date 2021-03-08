using System.Linq;

namespace Skybrud.Umbraco.Search.Options {

    public static class QueryListExtensions {

        public static T AppendNodeTypeAlias<T>(this T list, string alias) where T : QueryList {
            list.Add($"__NodeTypeAlias:{alias}");
            return list;
        }
        
        public static T AppendNodeTypeAliases<T>(this T list, params string[] aliases) where T : QueryList {
            list.Add($"__NodeTypeAlias:({string.Join(" ", aliases)})");
            return list;
        }

        public static void AppendNodeTypeAliases(this QueryList list, ContentTypeList contentTypes) {
            if (contentTypes == null || contentTypes.Count == 0) return;
            list.Add($"__NodeTypeAlias:({string.Join(" ", contentTypes.ToArray())})");
        }

        public static T AppendHideFromSearch<T>(this T list) where T : QueryList {
            list.Add("hideFromSearch:0");
            return list;
        }

        public static T AppendAncestor<T>(this T list, int ancestorId) where T : QueryList {
            list.Add($"path_search:{ancestorId}");
            return list;
        }

        public static T AppendAncestors<T>(this T list, params int[] ancestorIds) where T : QueryList {
            list.Add($"path_search:({string.Join(" ", from id in ancestorIds select id)})");
            return list;
        }

        public static T Append<T>(this T list, QueryList query) where T : QueryList {
            list.Add(query);
            return list;
        }

    }

}