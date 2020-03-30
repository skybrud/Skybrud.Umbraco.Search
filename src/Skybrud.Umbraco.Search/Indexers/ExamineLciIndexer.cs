using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Providers;
using IndexTypes = UmbracoExamine.IndexTypes;

namespace Skybrud.Umbraco.Search.Indexers
{
    public class ExamineLciIndexer
    {
        public ExamineLciIndexer(string indexName = "ExternalIndexer")
        {
            BaseIndexProvider externalIndexer = ExamineManager.Instance.IndexProviderCollection[indexName];
            externalIndexer.GatheringNodeData += externalIndexer_GatheringNodeData;
        }

        private void externalIndexer_GatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            if (e.IndexType != IndexTypes.Media)
            {
                // lci index for easy search
                IEnumerable<KeyValuePair<string, string>> lcifields = e.Fields.Where(a => !a.Key.EndsWith("_lci"));

                var lci = new Dictionary<string, string>();

                foreach (var field in lcifields)
                {
                    lci[field.Key + "_lci"] = field.Value.ToLowerInvariant();
                }

                foreach (var field in lci)
                    e.Fields[field.Key] = field.Value;
            }

        }
    }
}