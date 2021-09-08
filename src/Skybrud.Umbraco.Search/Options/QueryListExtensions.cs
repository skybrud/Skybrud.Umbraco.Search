using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.Umbraco.Search.Constants;

namespace Skybrud.Umbraco.Search.Options {

    /// <summary>
    /// Static class with various extension methods for <see cref="QueryList"/>.
    /// </summary>
    public static class QueryListExtensions {

        /// <summary>
        /// Appends the query with the specified <paramref name="nodeTypeAlias"/> to the query list.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="nodeTypeAlias">The alias of the node type.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendNodeTypeAlias<T>(this T list, string nodeTypeAlias) where T : QueryList {
            if (string.IsNullOrWhiteSpace(nodeTypeAlias)) throw new ArgumentNullException(nameof(nodeTypeAlias));
            list?.Add($"{ExamineConstants.Fields.NodeTypeAlias}:{nodeTypeAlias}");
            return list;
        }
        
        /// <summary>
        /// Appends a new OR query for matching one of the specified <paramref name="nodeTypeAliases"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="nodeTypeAliases">The aliases of the node types.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendNodeTypeAliases<T>(this T list, params string[] nodeTypeAliases) where T : QueryList {
            if (nodeTypeAliases == null || nodeTypeAliases.Length == 0) return list;
            list?.Add($"{ExamineConstants.Fields.NodeTypeAlias}:({string.Join(" ", nodeTypeAliases)})");
            return list;
        }
        
        /// <summary>
        /// Appends a new OR query for matching one of the specified <paramref name="nodeTypeAliases"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="nodeTypeAliases">The aliases of the node types.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendNodeTypeAliases<T>(this T list, IEnumerable<string> nodeTypeAliases) where T : QueryList {
            return nodeTypeAliases == null ? list : AppendNodeTypeAliases(list, nodeTypeAliases.ToArray());
        }
        
        /// <summary>
        /// Appends a new OR query for matching one of the specified <paramref name="contentTypes"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="contentTypes">A list of content types.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendNodeTypeAliases<T>(this T list, ContentTypeList contentTypes) where T : QueryList {
            if (contentTypes == null || contentTypes.Count == 0) return list;
            list?.Add($"{ExamineConstants.Fields.NodeTypeAlias}:({string.Join(" ", contentTypes.ToArray())})");
            return list;
        }

        /// <summary>
        /// Appends a new query requiring that results doesn't have a flag that they should be hidden from search results.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendHideFromSearch<T>(this T list) where T : QueryList {
            list?.Add($"{ExamineConstants.Fields.HideFromSearch}:0");
            return list;
        }

        /// <summary>
        /// Appends a new query requiring that the returned result are a descendant of or equal to the node with the specified <paramref name="ancestorId"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="ancestorId">The ID of the ancestor.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendAncestor<T>(this T list, int ancestorId) where T : QueryList {
            list?.Add($"{ExamineConstants.Fields.PathSearch}:{ancestorId}");
            return list;
        }

        /// <summary>
        /// Appends a new OR query requiring that returned results are a descendant or equal to at least one of the nodes matching the specified <paramref name="ancestorIds"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="ancestorIds">The IDs of the ancestors.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendAncestors<T>(this T list, params int[] ancestorIds) where T : QueryList {
            if (ancestorIds == null || ancestorIds.Length == 0) return list;
            list?.Add($"{ExamineConstants.Fields.PathSearch}:({string.Join(" ", from id in ancestorIds select id)})");
            return list;
        }
        
        /// <summary>
        /// Appends a new OR query requiring that returned results are a descendant or equal to at least one of the nodes matching the specified <paramref name="ancestorIds"/>.
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list.</param>
        /// <param name="ancestorIds">The IDs of the ancestors.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T AppendAncestors<T>(this T list, IEnumerable<int> ancestorIds) where T : QueryList {
            return ancestorIds == null ? null : AppendAncestors(list, ancestorIds.ToArray());
        }

        /// <summary>
        /// Appends the specified <paramref name="queryList"/> to the query list
        /// </summary>
        /// <typeparam name="T">The type of the query list.</typeparam>
        /// <param name="list">The query list to which <paramref name="queryList"/> should be added.</param>
        /// <param name="queryList">The query list to be added.</param>
        /// <returns><paramref name="list"/> - useful for method chaining.</returns>
        public static T Append<T>(this T list, QueryList queryList) where T : QueryList {
            list?.Add(queryList);
            return list;
        }

    }

}