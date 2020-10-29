using Newtonsoft.Json;
using Skybrud.Essentials.Strings;

namespace Skybrud.Umbraco.Search.Models.Forms{

    public class SearchFieldBase {

        #region Properties

        [JsonIgnore]
        public SearchFieldType Type { get; set; }

        [JsonProperty("type", Order = -99)]
        public string TypeString => StringUtils.ToCamelCase(Type);

        [JsonProperty("name", Order = -98, NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("label", Order = -97, NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("description", Order = -96, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("required", Order = -95, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsRequired { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore, Order = 500)]
        public object Value { get; set; }

        #endregion

        #region Constructors

        public SearchFieldBase() { }

        public SearchFieldBase(SearchFieldType type, string name) {
            Type = type;
            Name = name;
        }

        public SearchFieldBase(SearchFieldType type, string name, object value) {
            Type = type;
            Name = name;
            Value = value;
        }

        #endregion

        #region Static methods

        public static SearchFieldBase Hidden(string name, object value) {
            return new SearchFieldBase(SearchFieldType.Hidden, name, value);
        }

        #endregion
    }

}