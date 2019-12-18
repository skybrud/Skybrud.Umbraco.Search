namespace Skybrud.Umbraco.Search.Options.Fields {

    public class Field {
        
        #region Properties

        public string FieldName { get; set; }

        public int? Boost { get; set; }

        public float? Fuzz { get; set; }

        #endregion

        #region Constructors

        public Field(string fieldName) {
            FieldName = fieldName;
            Boost = null;
            Fuzz = null;
        }

        public Field(string fieldName, int? boost) {
            FieldName = fieldName;
            Boost = boost;
        }

        public Field(string fieldName, int? boost, float? fuzz) {
            FieldName = fieldName;
            Boost = boost;
            Fuzz = fuzz;
        }

        #endregion

    }

}