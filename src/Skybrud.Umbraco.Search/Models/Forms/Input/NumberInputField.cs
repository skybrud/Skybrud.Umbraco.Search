using Newtonsoft.Json;

namespace Skybrud.Umbraco.Search.Models.Forms.Input {

    public class NumberInputField : InputField {

        [JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
        public int? Min { get; set; }

        [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
        public int? Max { get; set; }

        public NumberInputField() : base(SearchFieldType.Number) { }

    }

}