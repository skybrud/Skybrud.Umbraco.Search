using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis;
using Lucene.Net.QueryParsers;
using Skybrud.Umbraco.Search.Models.Interfaces;

namespace Skybrud.Umbraco.Search.Models.Options
{
    public class SearchOptions : IPaginatedExamineOptions
    {
        #region Properties

        public int Offset { get; set; }
        public int Limit { get; set; }

        public string Text { get; set; }
        public int[] RootIds { get; set; }
        public string[] DocumentTypes { get; set; }
        public string ExamineIndex { get; set; }
        public FieldList Fields { get; set; }
        public Order Order { get; set; }
        public DateRange DateRange { get; set; }
        public ISpecificFields SpecificFields { get; set; }
        public string CustomRawSearchString { get; set; }
        public bool DisableHideFromSearch { get; set; }
        public bool Debug { get; set; }

        #endregion

        #region Constructors

        public SearchOptions()
        {
            Text = "";
            RootIds = null;
            DocumentTypes = new string[] {};
            ExamineIndex = "ExternalSearcher";
            Fields = new FieldList();
            Order = new Order();
            DateRange = new DateRange();
            SpecificFields = new SpecificFields();
            CustomRawSearchString = "";
            DisableHideFromSearch = false;
            Debug = false;
        }

        public SearchOptions(
            string keywords = "", 
            int[] rootIds = null,
            string[] documentTypes = null,
            string examineIndex = "ExternalSearcher",
            FieldList fields = null,
            Order order = null,
            DateRange dateRange = null,
            ISpecificFields specificFields = null,
            string customRawSearchString = "",
            bool disableHideFromSearch = false,
            bool debug = false)
        {
            Text = keywords;
            RootIds = rootIds;
            DocumentTypes = documentTypes;
            ExamineIndex = examineIndex;
            Fields = fields;
            Order = order;
            DateRange = dateRange;
            SpecificFields = specificFields;
            CustomRawSearchString = customRawSearchString;
            DisableHideFromSearch = disableHideFromSearch;
            Debug = debug;
        }

        #endregion

        #region Static Methods

        public static SearchOptions GetSearchOptions(
            string keywords,
            int[] rootIds,
            string[] documentTypes,
            string examineIndex,
            FieldList fields,
            Order order,
            DateRange dateRangeOptions,
            ISpecificFields specificFields,
            string customRawSearchString,
            bool disableHideFromSearch,
            bool debug)
        {
            return new SearchOptions(keywords, rootIds, documentTypes, examineIndex, fields, order, dateRangeOptions, specificFields, customRawSearchString, disableHideFromSearch, debug);
        }

        #endregion

        #region Members

        public virtual List<string> GetQueryList()
        {
            var query = new List<string>();

            //doctypes
            if (DocumentTypes != null && DocumentTypes.Any())
                query.Add(string.Format("nodeTypeAlias:({0})",
                    string.Join(" ",
                        DocumentTypes.Select(a => string.Format("\"{0}\"", QueryParser.Escape(a))).ToArray())));

            //keywords
            if (!string.IsNullOrEmpty(Text))
            {
                Text = Regex.Replace(Text, @"[^\wæøåÆØÅ\- ]", "").ToLowerInvariant().Trim();
                string[] terms = Text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                // fjerner stop-ord fra søgetermen
                terms = terms.Where(x => !StopAnalyzer.ENGLISH_STOP_WORDS_SET.Contains(x.ToLower())).ToArray();

                // fallback if no fields are added
                Fields =
                    Fields ??
                    FieldList.GetFromStringArray(new[] {"nodeName_lci", "contentTeasertext_lci", "contentBody_lci"});

                query.Add(Fields.GetQuery(terms));
            }

            //dateRange
            if (DateRange != null && DateRange.IsValid)
            {
                query.Add(DateRange.GetQuery());
            }

            //csv categorysearch
            if (SpecificFields != null && SpecificFields.IsValid)
            {
                query.AddRange(SpecificFields.GetQuery());
            }

            //site limit
            if (RootIds != null && RootIds.Any())
            {
                query.Add("(" + string.Join(" OR ", from id in RootIds select " +path_search:" + id) + ")");
            }

            //hideFromSearch
            if (!DisableHideFromSearch)
            {
                query.Add("hideFromSearch:(0)");
            }

            // extra search query
            if (!string.IsNullOrWhiteSpace(CustomRawSearchString))
            {
                query.Add(CustomRawSearchString);
            }
            return query;
        }
        public virtual string GetRawQuery()
        {
            return string.Join(" AND ", GetQueryList().ToArray());
        }

        #endregion

    }
}
