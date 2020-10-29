namespace Skybrud.Umbraco.Search.Models.Forms {

    public class Button : SearchFieldBase {

        public Button() : base(SearchFieldType.Button, null, null) { }

        protected Button(SearchFieldType type) : base(type, null, null) { }

    }

}