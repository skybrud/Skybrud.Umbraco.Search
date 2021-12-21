namespace Skybrud.Umbraco.Search.Constants {
    
    public static class ExamineConstants {

        public const string ExternalIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.ExternalIndexName;

        public const string InternalIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.InternalIndexName;

        public const string MembersIndexName = global::Umbraco.Cms.Core.Constants.UmbracoIndexes.MembersIndexName;

        /// <summary>
        /// Gets the name of the index created by the <strong>UmbracoCms.UmbracoExamine.PDF</strong> package.
        /// </summary>
        /// <see>
        ///     <cref>https://www.nuget.org/packages/UmbracoCms.UmbracoExamine.PDF/</cref>
        /// </see>
        public const string PdfIndexName = "PDFIndex";

        public static class Fields {

            public const string IndexType = "__IndexType";

            public const string Key = "__Key";

            public const string NodeId = "__NodeId";

            public const string NodeTypeAlias = "__NodeTypeAlias";

            public const string Path = "path";

            public const string PathSearch = "path_search";

            public const string HideFromSearch = "hideFromSearch";

        }

    }

}