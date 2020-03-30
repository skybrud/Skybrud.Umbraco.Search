using System;
using System.Collections.Generic;
using Examine;
using Examine.Providers;

namespace Skybrud.Umbraco.Search.Indexers
{
    public class ExamineDateRangeIndexer
    {
        /// <summary>
        ///     RangeIndexer is used to convert datetime-fields to be searchable with Lucenes range-search. The new fieldname will
        ///     be [oldFieldname]_range
        /// </summary>
        /// <param name="rangePropertyNames">string-array containing the names of the fields to enable range-index on</param>
        public ExamineDateRangeIndexer(string[] rangePropertyNames, string indexName = "ExternalIndexer")
        {
            BaseIndexProvider externalIndexer = ExamineManager.Instance.IndexProviderCollection[indexName];

            externalIndexer.GatheringNodeData +=
                (sender, args) => externalIndexer_GatheringNodeData(sender, args, rangePropertyNames);
        }

        private void externalIndexer_GatheringNodeData(object sender, IndexingNodeDataEventArgs e,
            IEnumerable<string> rangePropertyNames)
        {
            foreach (string rangePropertyName in rangePropertyNames)
            {
                if (!e.Fields.ContainsKey(rangePropertyName)) continue;
                string rangeFieldName = string.Format("{0}_range", rangePropertyName);
                e.Fields.Add(rangeFieldName, DateTime.Parse(e.Fields[rangePropertyName]).ToString("yyyyMMddHHmm00000"));
            }
        }
    }
}