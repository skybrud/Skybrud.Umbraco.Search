namespace Skybrud.Umbraco.Search.Models.Options
{
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

        public Field(string fieldName, int? boost = null, float? fuzz = null) {
            FieldName = fieldName;
            Boost = boost;
            Fuzz = fuzz;
        }

        #endregion

        #region Static Methods

        public static Field GetFromString(string fieldName) {
            return new Field(fieldName);
        }

        public static Field GetFieldOption(string fieldName, int? boost, float? fuzz) {
            return new Field(fieldName, boost, fuzz);
        }

        #endregion

    }

}