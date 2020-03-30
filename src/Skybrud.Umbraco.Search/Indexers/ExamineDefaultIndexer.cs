using Examine;
using Examine.Providers;
using UmbracoExamine;

namespace Skybrud.Umbraco.Search.Indexers
{
    public class ExamineDefaultIndexer
    {
        public ExamineDefaultIndexer(string indexName = "ExternalIndexer")
        {
            BaseIndexProvider externalIndexer = ExamineManager.Instance.IndexProviderCollection[indexName];
            externalIndexer.GatheringNodeData += externalIndexer_GatheringNodeData;
        }

        private void externalIndexer_GatheringNodeData(object sender, IndexingNodeDataEventArgs e)
        {
            //only on contentnodes
            if (e.IndexType != IndexTypes.Content) return;

            // make path searchable
            if (!e.Fields.ContainsKey("path")) return;
            string path = e.Fields["path"];
            path = path.Replace(',', ' ');
            e.Fields.Add("path_search", path);

            // create defaultvalue "hideFromSearch"
            if (!e.Fields.ContainsKey("hideFromSearch"))
            {
                e.Fields["hideFromSearch"] = "0";
            }
        }
    }
}