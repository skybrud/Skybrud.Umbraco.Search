using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.QueryParsers;

namespace Skybrud.Umbraco.Search.Options.Fields {

    public class FieldList : IEnumerable<Field> {
        
        private readonly List<Field> _fields;

        #region Properties
        
        public bool HasBoostValues {
            get { return _fields.Any(x => x.Boost != null && x.Boost != 0); }
        }

        public bool HasFuzzyValues {
            get { return _fields.Any(x => x.Fuzz != null && x.Fuzz > 0 && x.Fuzz < 1); }
        }

        public bool IsValid => _fields != null && _fields.Any();

        #endregion

        #region Constructors

        public FieldList() {
            _fields = new List<Field>();
        }

        private FieldList(string[] fields)
        {
            var fieldOptions = new List<Field>();
            foreach (var fieldName in fields)
            {
                fieldOptions.Add(new Field(fieldName));
            }
            _fields = fieldOptions;
        }

        private FieldList(string[] fields, int?[] boosts, float?[] fuzzies) {
            var fieldOptions = new List<Field>();
            for (int i = 0; i < fields.Length; i++) {
                fieldOptions.Add(new Field(fields[i], boosts[i], fuzzies[i]));
            }
            _fields = fieldOptions;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Use this for creating a new instance of the fieldoptions class where only the fieldNames are set
        /// </summary>
        /// <param name="fieldNames"></param>
        /// <returns></returns>
        public static FieldList GetFromStringArray(string[] fieldNames) {
            return new FieldList(fieldNames);
        }

        public static FieldList GetFieldOptions(string[] fields, int?[] boosts, float?[] fuzzies) {
            return new FieldList(fields,boosts,fuzzies);
        }

        #endregion

        #region Members

        public void Add(Field field) {
            _fields.Add(field);
        }

        public void Add(string fieldName, int? boost = null, float? fuzz = null) {
            _fields.Add(new Field(fieldName, boost, fuzz));
        }

        public void AddRange(params Field[] fields) {
            _fields.AddRange(fields);
        }

        public void AddRange(IEnumerable<Field> fields) {
            _fields.AddRange(fields);
        }

        public virtual string GetQuery(string[] terms) {

            List<string> searchTerms = new List<string>();

            foreach (string term in terms) {

                string escapedTerm = QueryParser.Escape(term);
                string t = "(";

                if (IsValid) {
                    
                    // Boost
                    if (HasBoostValues) {
                        t += string.Join(" OR ", _fields.Where(x => x.Boost != null).Select(fieldOption => string.Format("{0}:({1} {1}*)^{2}", fieldOption.FieldName, escapedTerm, fieldOption.Boost.ToString())).ToArray());
                        t += " OR ";
                    }

                    // Fuzzy
                    if (HasFuzzyValues) {
                        t += string.Join(" OR ", _fields.Where(x => x.Fuzz != null && x.Fuzz > 0 && x.Fuzz < 1).Select(fieldOption => string.Format("{0}:{1}~{2}", fieldOption.FieldName, escapedTerm, fieldOption.Fuzz.ToString())).ToArray());
                        t += " OR ";
                    }

                    // Add regular search
                    t += string.Join(" OR ", _fields.Select(fieldOption => string.Format("{1}:({0} {0}*)", escapedTerm, fieldOption.FieldName)).ToArray());
                }

                t += ")";
                searchTerms.Add(t);

            }

            return string.Join(" AND ", searchTerms.ToArray());

        }
        
        public IEnumerator<Field> GetEnumerator() {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

    }

}