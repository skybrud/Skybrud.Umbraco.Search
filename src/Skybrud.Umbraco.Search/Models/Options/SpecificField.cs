using System.ComponentModel;
using System.Linq;
using Skybrud.Umbraco.Search.Models.Interfaces;

namespace Skybrud.Umbraco.Search.Models.Options
{
    public class SpecificField : ISpecificField
    {
        #region Properties

        public string FieldName { get; set; }
        public string[] SearchTerms { get; set; }
        [Description("The type of join used between the SearchTerms")]
        public JoinTypeEnum JoinType { get; set; }
        [Description("The type of join used with previous SpecificField")]
        public JoinTypeEnum OuterJoinType { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(FieldName) && SearchTerms != null && SearchTerms.Any();

        #endregion

        #region Constructors

        public SpecificField()
        {
            FieldName = "";
            SearchTerms = null;
            JoinType = JoinTypeEnum.Or;
            OuterJoinType = JoinTypeEnum.Or;
        }

        private SpecificField(string fieldName, string[] searchTerms)
        {
            FieldName = fieldName;
            SearchTerms = searchTerms;
            JoinType = JoinTypeEnum.Or;
            OuterJoinType = JoinTypeEnum.Or;
        }

        private SpecificField(string fieldName, string[] searchTerms, JoinTypeEnum joinType = JoinTypeEnum.Or, JoinTypeEnum outerJoinType = JoinTypeEnum.Or)
        {
            FieldName = fieldName;
            SearchTerms = searchTerms;
            JoinType = joinType;
            OuterJoinType = outerJoinType;
        }

        #endregion

        #region Static Methods

        public static SpecificField GetSpecificField(string fieldName, string[] searchTerms, JoinTypeEnum joinType = JoinTypeEnum.Or, JoinTypeEnum outerJoinType = JoinTypeEnum.Or)
        {
            return new SpecificField(fieldName, searchTerms, joinType, outerJoinType);
        }

        #endregion
    }
}
