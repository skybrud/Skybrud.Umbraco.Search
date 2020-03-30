namespace Skybrud.Umbraco.Search.Options {

    public interface IPaginatedSearchOptions : ISearchOptions {
        
        int Offset { get; set; }

        int Limit { get; set; }

    }

}