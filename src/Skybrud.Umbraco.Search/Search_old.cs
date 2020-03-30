//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using Examine;
//using Examine.Providers;
//using Examine.SearchCriteria;
//using Lucene.Net.Analysis;
//using Lucene.Net.QueryParsers;
//using Skybrud.Umbraco.Search.Models;
//using Umbraco.Core;
//using Umbraco.Core.Logging;
//using Umbraco.Core.Models;
//using Umbraco.Web;

//namespace Skybrud.Umbraco.Search
//{
//    public class SkybrudSearch
//    {
//        /// <summary>
//        ///     Search in your Umbraco Documents
//        /// </summary>
//        /// <param name="keywords">freetext words to search after</param>
//        /// <param name="documentTypes">documenttypes to search in</param>
//        /// <param name="fields">fieldnames to search in</param>
//        /// <param name="rootId">limit search by rootId</param>
//        /// <param name="limit">limit results</param>
//        /// <param name="offset">offset results</param>
//        /// <param name="orderBy">fieldname to order by (default: createDate)</param>
//        /// <param name="orderDirection">orderdirection (default: descending)</param>
//        /// <param name="orderByScore">if set to true, orderBy is skipped</param>
//        /// <param name="dateRangeFieldname">fieldname for daterange search</param>
//        /// <param name="dateRangeStart">startdate for daterange search</param>
//        /// <param name="dateRangeEnd">enddate for daterange search</param>
//        /// <param name="total">output total searchresults</param>
//        /// <param name="csvIdSearch">Dictionary for csv search. Keys are fieldnames. Values are search search terms</param>
//        /// <returns>IENumerable<IPublishedContent></returns>
//        public static IEnumerable<IPublishedContent> SearchDocuments(
//            string keywords,
//            out int total,
//            string[] documentTypes = null,
//            string[] fields = null,
//            int rootId = -1,
//            int limit = 10,
//            int offset = 0,
//            string orderBy = "createDate",
//            OrderType orderType = OrderType.String,
//            bool orderByScore = false,
//            OrderDirection orderDirection = OrderDirection.Descending,
//            string dateRangeFieldname = "",
//            DateTime dateRangeStart = new DateTime(),
//            DateTime dateRangeEnd = new DateTime(),
//            Dictionary<string, string[]> csvIdSearch = null,
//            string examineIndex = "ExternalSearcher",
//            Dictionary<string, int> fieldsBoosted = null,
//            Dictionary<string, string> fieldsFuzzy = null,
//            List<FieldOption> fieldOptions = null,
//            bool disableHideFromSearch = false,
//            bool debug = false
//            )
//        {
//            var results = SearchExamine(
//                    keywords,
//                    documentTypes,
//                    fields,
//                    rootId,
//                    orderBy,
//                    orderType,
//                    orderByScore,
//                    orderDirection,
//                    dateRangeFieldname,
//                    dateRangeStart,
//                    dateRangeEnd,
//                    csvIdSearch,
//                    examineIndex,
//                    fieldsBoosted,
//                    fieldsFuzzy,
//                    fieldOptions,
//                    disableHideFromSearch,
//                    debug);

//            total = results.Count();

//            return
//                results.Skip(offset)
//                    .Take(limit)
//                    .Select(a => UmbracoContext.Current.ContentCache.GetById(int.Parse(a.Fields["id"])));
//        }

//        public static IEnumerable<IPublishedContent> SearchDocuments(
//            string keywords,
//            out int total,
//            ref List<SearchGroup> grouping,
//            int[] searchInGroup = null,
//            string[] documentTypes = null,
//            string[] fields = null,
//            int rootId = -1,
//            int limit = 10,
//            int offset = 0,
//            string orderBy = "createDate",
//            OrderType orderType = OrderType.String,
//            bool orderByScore = false,
//            OrderDirection orderDirection = OrderDirection.Descending,
//            string dateRangeFieldname = "",
//            DateTime dateRangeStart = new DateTime(),
//            DateTime dateRangeEnd = new DateTime(),
//            Dictionary<string, string[]> csvIdSearch = null,
//            string examineIndex = "ExternalSearcher",
//            Dictionary<string, int> fieldsBoosted = null,
//            Dictionary<string, string> fieldsFuzzy = null,
//            List<FieldOption> fieldOptions = null,
//            bool disableHideFromSearch = false,
//            bool debug = false
//            )
//        {
//            IOrderedEnumerable<SearchResult> results = SearchExamine(
//                    keywords,
//                    documentTypes,
//                    fields,
//                    rootId,
//                    orderBy,
//                    orderType,
//                    orderByScore,
//                    orderDirection,
//                    dateRangeFieldname,
//                    dateRangeStart,
//                    dateRangeEnd,
//                    csvIdSearch,
//                    examineIndex,
//                    fieldsBoosted,
//                    fieldsFuzzy,
//                    fieldOptions,
//                    disableHideFromSearch,
//                    debug);

//            total = results.Count();

//            List<SearchResult> tempResults = results.ToList();  // arbejdskopi af resultater

//            //1. Returnere Grouping med korrekt Count (hvor mange af hver type doctype(s)/gruppe er der, resten ryger i Misc(hvis sat))
//            foreach (var sg in grouping.Where(x => !x.MiscResults))
//            {
//                if (sg.Doctypes != null && sg.Doctypes.Any()) // tæl og fjern alle resultater fra doctype
//                {
//                    var items = tempResults.Where(
//                            x => x.Fields.ContainsKey("nodeTypeAlias") &&
//                            sg.Doctypes.Any(y => x.Fields["nodeTypeAlias"] == y))
//                        .ToList();
//                    sg.AssociatedResults.AddRange(items);
//                    tempResults.RemoveAll(x => items.Contains(x));
//                }
//                if (!string.IsNullOrWhiteSpace(sg.ParentId)) // tæl og fjern alle resultater fra parentid
//                {
//                    var items = tempResults.Where(
//                            x => x.Fields.ContainsKey("path_search") &&
//                            x.Fields["path_search"].Split(' ').Any(y => sg.ParentId == y))
//                        .ToList();
//                    sg.AssociatedResults.AddRange(items);
//                    tempResults.RemoveAll(x => items.Contains(x));
//                }
//                sg.Count += sg.AssociatedResults.Count;
//            }
//            foreach (var sg in grouping.Where(x => x.MiscResults)) // har sit eget loop, da det ikke er sikkert den er til sidst.
//            {
//                sg.Count += tempResults.Count;
//            }

//            //2. Søge i specifikke grupper
//            if (searchInGroup != null && searchInGroup.Any() && grouping.Any())
//            {
//                var groupResults = new List<SearchResult>();

//                foreach (var i in searchInGroup)
//                {
//                    groupResults.AddRange(grouping.First(x => x.Id == i).AssociatedResults);
//                }
//                total = groupResults.Count;
//                grouping.ForEach(x => x.AssociatedResults.Clear());

//                return
//                        groupResults.Skip(offset)
//                            .Take(limit)
//                            .Select(a => UmbracoContext.Current.ContentCache.GetById(int.Parse(a.Fields["id"])));
//            }

//            grouping.ForEach(x => x.AssociatedResults.Clear());

//            // standard søgeresultat
//            return
//                results.Skip(offset)
//                    .Take(limit)
//                    .Select(a => UmbracoContext.Current.ContentCache.GetById(int.Parse(a.Fields["id"])));
//        }

//        public static IEnumerable<SearchResult> SearchDocumentsRawResults(
//           string keywords,
//           out int total,
//           string[] documentTypes = null,
//           string[] fields = null,
//           int rootId = -1,
//           int limit = 10,
//           int offset = 0,
//           string orderBy = "createDate",
//           OrderType orderType = OrderType.String,
//           bool orderByScore = false,
//           OrderDirection orderDirection = OrderDirection.Descending,
//           string dateRangeFieldname = "",
//           DateTime dateRangeStart = new DateTime(),
//           DateTime dateRangeEnd = new DateTime(),
//           Dictionary<string, string[]> csvIdSearch = null,
//           string examineIndex = "ExternalSearcher",
//           Dictionary<string, int> fieldsBoosted = null,
//           Dictionary<string, string> fieldsFuzzy = null,
//           List<FieldOption> fieldOptions = null,
//            bool disableHideFromSearch = false,
//           bool debug = false
//           )
//        {

//            var results = SearchExamine(
//                    keywords,
//                    documentTypes,
//                    fields,
//                    rootId,
//                    orderBy,
//                    orderType,
//                    orderByScore,
//                    orderDirection,
//                    dateRangeFieldname,
//                    dateRangeStart,
//                    dateRangeEnd,
//                    csvIdSearch,
//                    examineIndex,
//                    fieldsBoosted,
//                    fieldsFuzzy,
//                    fieldOptions,
//                    disableHideFromSearch,
//                    debug);

//            total = results.Count();

//            return
//                results.Skip(offset)
//                    .Take(limit);
//        }


//        public static IEnumerable<SearchResult> SearchDocumentsRawResults(
//            string keywords,
//            out int total,
//            ref List<SearchGroup> grouping,
//            int[] searchInGroup = null,
//            string[] documentTypes = null,
//            string[] fields = null,
//            int rootId = -1,
//            int limit = 10,
//            int offset = 0,
//            string orderBy = "createDate",
//            OrderType orderType = OrderType.String,
//            bool orderByScore = false,
//            OrderDirection orderDirection = OrderDirection.Descending,
//            string dateRangeFieldname = "",
//            DateTime dateRangeStart = new DateTime(),
//            DateTime dateRangeEnd = new DateTime(),
//            Dictionary<string, string[]> csvIdSearch = null,
//            string examineIndex = "ExternalSearcher",
//            Dictionary<string, int> fieldsBoosted = null,
//            Dictionary<string, string> fieldsFuzzy = null,
//            List<FieldOption> fieldOptions = null,
//            bool disableHideFromSearch = false,
//            bool debug = false
//           )
//        {
//            var results = SearchExamine(
//                keywords,
//                documentTypes,
//                fields,
//                rootId,
//                orderBy,
//                orderType,
//                orderByScore,
//                orderDirection,
//                dateRangeFieldname,
//                dateRangeStart,
//                dateRangeEnd,
//                csvIdSearch,
//                examineIndex,
//                fieldsBoosted,
//                fieldsFuzzy,
//                fieldOptions,
//                disableHideFromSearch,
//                debug);

//            total = results.Count();
//            List<SearchResult> tempResults = results.ToList();

//            //1. Returnere Grouping med korrekt Count (hvor mange af hver type doctype(s)/gruppe er der, resten ryger i Misc(hvis sat))
//            foreach (var sg in grouping.Where(x => !x.MiscResults))
//            {
//                if (sg.Doctypes != null && sg.Doctypes.Any()) // tæl og fjern alle resultater fra doctype
//                {
//                    var items = tempResults.Where(
//                        x => x.Fields.ContainsKey("nodeTypeAlias") &&
//                             sg.Doctypes.Any(y => x.Fields["nodeTypeAlias"] == y))
//                        .ToList();
//                    sg.AssociatedResults.AddRange(items);
//                    tempResults.RemoveAll(x => items.Contains(x));
//                }
//                if (!string.IsNullOrWhiteSpace(sg.ParentId))
//                {
//                    var items = tempResults
//                        .Where(x => x.Fields.ContainsKey("path_search") &&
//                                    x.Fields["path_search"].Split(' ').Any(y => sg.ParentId == y))
//                        .ToList();
//                    sg.AssociatedResults.AddRange(items);
//                    tempResults.RemoveAll(x => items.Contains(x));
//                }
//                sg.Count += sg.AssociatedResults.Count;
//            }
//            foreach (var sg in grouping.Where(x => x.MiscResults)) // har sit eget loop, da det ikke er sikkert den er til sidst.
//            {
//                sg.Count += tempResults.Count;
//            }

//            //2. Søge i specifikke grupper
//            if (searchInGroup != null && searchInGroup.Any() && grouping.Any())
//            {
//                var groupResults = new List<SearchResult>();
//                foreach (var i in searchInGroup)
//                {
//                    groupResults.AddRange(grouping.First(x => x.Id == i).AssociatedResults);
//                }
//                total = groupResults.Count;
//                grouping.ForEach(x => x.AssociatedResults.Clear());

//                return
//                    groupResults
//                        .Skip(offset)
//                        .Take(limit);
//            }
//            grouping.ForEach(x => x.AssociatedResults.Clear());

//            return
//                results
//                    .Skip(offset)
//                    .Take(limit);
//        }

//        private static IOrderedEnumerable<SearchResult> SearchExamine(
//            string keywords,
//            string[] documentTypes,
//            string[] fields,
//            int rootId,
//            string orderBy,
//            OrderType orderType,
//            bool orderByScore,
//            OrderDirection orderDirection,
//            string dateRangeFieldname,
//            DateTime dateRangeStart,
//            DateTime dateRangeEnd,
//            Dictionary<string, string[]> csvIdSearch,
//            string examineIndex,
//            Dictionary<string, int> fieldsBoosted,
//            Dictionary<string, string> fieldsFuzzy,
//            List<FieldOption> fieldOptions,
//            // List<Tuple<string, int?, string>> fieldBoostFuzzy,
//            bool disableHideFromSearch,
//            bool debug
//            )
//        {
//            var query = new List<string>();
//            DateTime timeSpendTime = DateTime.Now;

//            //doctypes
//            if (documentTypes != null)
//                query.Add(string.Format("nodeTypeAlias:({0})",
//                    string.Join(" ",
//                        documentTypes.Select(a => string.Format("\"{0}\"", QueryParser.Escape(a))).ToArray())));

//            //keywords
//            if (!string.IsNullOrEmpty(keywords))
//            {
//                keywords = Regex.Replace(keywords, @"[^\wæøåÆØÅ\- ]", "").ToLowerInvariant().Trim();
//                string[] terms = keywords.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

//                // fjerner stop-ord fra søgetermen
//                terms = terms.Where(x => !StopAnalyzer.ENGLISH_STOP_WORDS_SET.Contains(x.ToLower())).ToArray();

//                //fallback if no fields are added
//                fields = fields ?? new[] { "nodeName_lci", "contentTeasertext_lci", "contentBody_lci" };

//                List<string> searchTerms = new List<string>();
//                foreach (var term in terms)
//                {
//                    string escapedTerm = QueryParser.Escape(term);
//                    string t = "(";

//                    //Handle boosted fields if any
//                    if (fieldsBoosted != null)
//                    {
//                        var boostedFieldsArray = fieldsBoosted.Select(x => x.Key).ToArray();

//                        t += string.Join(" OR ",
//                            boostedFieldsArray.Select(
//                                b => string.Format(
//                                        "{1}:({0} {0}*)^" + fieldsBoosted.FirstOrDefault(x => x.Key == b).Value
//                                            .ToString(),
//                                        escapedTerm, b)).ToArray());
//                        t += " OR ";
//                    }

//                    //Handle fuzzy
//                    if (fieldsFuzzy != null)
//                    {
//                        string[] fuzzyFieldsArray = fieldsFuzzy.Select(x => x.Key).ToArray();

//                        t += string.Join(" OR ",
//                            fuzzyFieldsArray.Select(
//                                b => string.Format(
//                                    "{0}:{1}~{2}",
//                                    b,
//                                    escapedTerm,
//                                    fieldsFuzzy.FirstOrDefault(x => x.Key == b).Value)).ToArray());

//                        t += " OR ";
//                    }

//                    // Handle Boost and fuzzy
//                    if (fieldOptions != null)
//                    {
//                        t += string.Join(
//                            " OR ",
//                            fieldOptions.Select(
//                                x => string.Format("{0}:({1} {2} {1}*){3}",
//                                    x.FieldName,
//                                    escapedTerm,
//                                    x.Fuzz.IsNullOrWhiteSpace() ? "" : escapedTerm + "~" + x.Fuzz,
//                                    x.Boost > 0 ? "^" + x.Boost : "")
//                            )
//                        );
//                        t += " OR ";
//                    }

//                    t += string.Join(" OR ",
//                                fields.Select(b => string.Format("{1}:({0} {0}*)", escapedTerm, b)).ToArray());

//                    t += ")";
//                    searchTerms.Add(t);
//                }
//                query.Add(string.Join(" AND ", searchTerms.ToArray()));
//            }

//            //dateRange
//            if (!string.IsNullOrEmpty(dateRangeFieldname))
//            {
//                const string dateFormat = "yyyyMMddHHmm00000";

//                string dateStart = dateRangeStart != DateTime.MinValue
//                    ? dateRangeStart.ToString(dateFormat)
//                    : "19000101000000000";
//                string dateEnd = dateRangeEnd != DateTime.MinValue
//                    ? dateRangeEnd.ToString(dateFormat)
//                    : "99999999999999999";

//                query.Add(string.Format(" +({0}:[{1} TO {2}])", dateRangeFieldname, dateStart, dateEnd));
//            }

//            ////csv categorysearch
//            if (csvIdSearch != null && csvIdSearch.Count > 0)
//            {
//                foreach (KeyValuePair<string, string[]> pair in csvIdSearch)
//                {
//                    string fieldName = pair.Key;
//                    string[] searchValues = pair.Value;
//                    if (!string.IsNullOrEmpty(fieldName) && searchValues != null && searchValues.Length > 0)
//                    {
//                        query.Add(string.Format("{0}:({1})", fieldName,
//                            string.Join(" ",
//                                searchValues.Select(a => string.Format("\"{0}\"", QueryParser.Escape(a))).ToArray())));
//                    }
//                }
//            }

//            //site limit
//            if (rootId > -1)
//            {
//                query.Add(string.Format(" +path_search:{0}", rootId));
//            }

//            //hideFromSearch
//            if (!disableHideFromSearch)
//            {
//                query.Add(string.Format("hideFromSearch:({0})", "0"));
//            }

//            //search
//            BaseSearchProvider externalSearcher = ExamineManager.Instance.SearchProviderCollection[examineIndex];
//            ISearchCriteria criteria = externalSearcher.CreateSearchCriteria();
//            criteria = criteria.RawQuery(string.Join(" AND ", query.ToArray()));


//            IOrderedEnumerable<SearchResult> results;

//            if (orderByScore)
//            {
//                results = orderDirection == OrderDirection.Descending
//                    ? externalSearcher.Search(criteria)
//                        .OrderByDescending(o => o.Score)
//                    : externalSearcher.Search(criteria)
//                        .OrderBy(o => o.Score);
//            }
//            else if (orderType == OrderType.Int)
//            {
//                results = orderDirection == OrderDirection.Descending
//                    ? externalSearcher.Search(criteria).Where(x => x.Fields.ContainsKey(orderBy))
//                        .OrderByDescending(o => Convert.ToInt32(o[orderBy]))
//                    : externalSearcher.Search(criteria).Where(x => x.Fields.ContainsKey(orderBy))
//                        .OrderBy(o => Convert.ToInt32(o[orderBy]));
//            }
//            else
//            {
//                results = orderDirection == OrderDirection.Descending
//                    ? externalSearcher.Search(criteria)
//                        .OrderByDescending(o => o.Fields.ContainsKey(orderBy) ? o.Fields[orderBy] : o.Fields["createDate"])
//                    : externalSearcher.Search(criteria)
//                        .OrderBy(o => o.Fields.ContainsKey(orderBy) ? o.Fields[orderBy] : o.Fields["createDate"]);
//            }

//            var TimeSpend = DateTime.Now.Subtract(timeSpendTime).TotalSeconds.ToString();

//            if (debug)
//            {
//                LogHelper.Info<SkybrudSearch>(TimeSpend + " q:" + string.Join(" AND ", query.ToArray()));

//            }

//            return results;
//        }
//    }

//    public enum OrderDirection
//    {
//        Ascending,
//        Descending
//    }

//    public enum OrderType
//    {
//        String,
//        Int
//    }

//}