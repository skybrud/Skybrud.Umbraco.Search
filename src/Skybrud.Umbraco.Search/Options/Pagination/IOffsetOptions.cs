namespace Skybrud.Umbraco.Search.Options.Pagination {

    public interface IOffsetOptions : ISearchOptions {

        int Offset { get; set; }

        int Limit { get; set; }

    }

}