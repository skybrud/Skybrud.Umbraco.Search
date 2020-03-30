using Skybrud.Umbraco.Search.Models.Options;

namespace Skybrud.Umbraco.Search.Models.Interfaces
{
    public interface ISpecificField
    {
        string FieldName { get; set; }
        string[] SearchTerms { get; set; }
        JoinTypeEnum JoinType { get; set; }
        JoinTypeEnum OuterJoinType { get; set; }

        bool IsValid { get; }

    }
}
