using System;
using System.Collections.Generic;
using System.Linq;

namespace Skybrud.Umbraco.Search.Options {

    public static class QueryListExtensions {

        public static T AppendNodeTypeAlias<T>(this T list, string alias) where T : QueryList {
            if (string.IsNullOrWhiteSpace(alias)) throw new ArgumentNullException(nameof(alias));
            list?.Add($"__NodeTypeAlias:{alias}");
            return list;
        }
        
        public static T AppendNodeTypeAliases<T>(this T list, params string[] aliases) where T : QueryList {
            if (aliases == null || aliases.Length == 0) return list;
            list?.Add($"__NodeTypeAlias:({string.Join(" ", aliases)})");
            return list;
        }

        public static T AppendNodeTypeAliases<T>(this T list, IEnumerable<string> aliases) where T : QueryList {
            return aliases == null ? list : AppendNodeTypeAliases(list, aliases.ToArray());
        }

        public static T AppendNodeTypeAliases<T>(this T list, ContentTypeList contentTypes) where T : QueryList {
            if (contentTypes == null || contentTypes.Count == 0) return list;
            list?.Add($"__NodeTypeAlias:({string.Join(" ", contentTypes.ToArray())})");
            return list;
        }

        public static T AppendHideFromSearch<T>(this T list) where T : QueryList {
            list?.Add("hideFromSearch:0");
            return list;
        }

        public static T AppendAncestor<T>(this T list, int ancestorId) where T : QueryList {
            list?.Add($"path_search:{ancestorId}");
            return list;
        }

        public static T AppendAncestors<T>(this T list, params int[] ancestorIds) where T : QueryList {
            list?.Add($"path_search:({string.Join(" ", from id in ancestorIds select id)})");
            return list;
        }

        public static T AppendAncestors<T>(this T list, IEnumerable<int> ancestorIds) where T : QueryList {
            return ancestorIds == null ? null : AppendAncestors(list, ancestorIds.ToArray());
        }

        public static T Append<T>(this T list, QueryList query) where T : QueryList {
            list?.Add(query);
            return list;
        }

    }

}