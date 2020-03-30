using System.Collections.Generic;
using System.Linq;
using Lucene.Net.QueryParsers;
using Skybrud.Umbraco.Search.Models.Interfaces;

namespace Skybrud.Umbraco.Search.Models.Options
{
    public class SpecificFields : ISpecificFields
    {
        #region Properties

        public List<ISpecificField> Fields { get; set; }

        public bool UseOuterJoins { get; set; }

        public bool IsValid => Fields != null && Fields.Count > 0;

        #endregion

        #region Constructors

        public SpecificFields()
        {
            Fields = new List<ISpecificField>();
            UseOuterJoins = false;
        }

        private SpecificFields(string fieldName, string[] searchTerms, JoinTypeEnum joinType = JoinTypeEnum.Or)
        {
            Fields = new List<ISpecificField> { SpecificField.GetSpecificField(fieldName, searchTerms, joinType) };
        }

        private SpecificFields(List<ISpecificField> list, bool useOuterJoins)
        {
            Fields = list;
            UseOuterJoins = useOuterJoins;
        }

        #endregion

        #region Static Methods

        public SpecificFields GetSingleIdSearch(string fieldName, string[] searchTerms, JoinTypeEnum joinType = JoinTypeEnum.Or)
        {
            return new SpecificFields(fieldName, searchTerms, joinType);
        }

        public SpecificFields GetFromList(List<ISpecificField> list, bool useOuterJoins = false)
        {
            return new SpecificFields(list, useOuterJoins);
        }

        #endregion

        #region Members

        public virtual IEnumerable<string> GetQuery()
        {
            List<string> returnValue = new List<string>();
            string tempValue = "(";

            if (IsValid)
            {

                foreach (var field in Fields.Where(x => x.IsValid))
                {
                    if (!field.IsValid) continue;
                    if (UseOuterJoins)
                    {
                        //var f = field == Fields[0];
                        tempValue += string.Format("{0}{1}:({2})",
                            field != Fields[0] ? field.OuterJoinType.JoinTypeToString() : "",
                            field.FieldName,
                            string.Join(field.JoinType.JoinTypeToString(),
                                field.SearchTerms.Select(a => string.Format("\"{0}\"", QueryParser.Escape(a))).ToArray()));
                    }
                    else
                    {
                        returnValue.Add(string.Format("{0}:({1})",
                            field.FieldName,
                            string.Join(field.JoinType.JoinTypeToString(),
                                field.SearchTerms.Select(a => string.Format("\"{0}\"", QueryParser.Escape(a))).ToArray())));
                    }
                }
            }
            tempValue += ")";
            return !UseOuterJoins ?  returnValue : new List<string> {tempValue};
        }

        #endregion
    }
}
