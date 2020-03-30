using System.Collections.Generic;

namespace Skybrud.Umbraco.Search.Models.Interfaces
{
    

    public interface ISpecificFields
    {
        List<ISpecificField> Fields { get; set; }
        bool IsValid { get; }

        bool UseOuterJoins { get; set; }


        IEnumerable<string> GetQuery();
    }
}
