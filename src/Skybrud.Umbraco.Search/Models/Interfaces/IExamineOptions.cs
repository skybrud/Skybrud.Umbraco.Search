using Skybrud.Umbraco.Search.Models.Options;

namespace Skybrud.Umbraco.Search.Models.Interfaces {

    public interface IExamineOptions {

        string ExamineIndex { get; set; }

        Order Order { get; set; }

        bool DisableHideFromSearch { get; set; }

        bool Debug { get; set; }

        /// <summary>
        /// Gets the raw query for the search.
        /// </summary>
        /// <returns></returns>
        string GetRawQuery();

    }

    public interface IPaginatedExamineOptions : IExamineOptions {
        
        int Offset { get; set; }

        int Limit { get; set; }

    }

}