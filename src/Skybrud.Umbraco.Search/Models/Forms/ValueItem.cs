using Newtonsoft.Json;

namespace Skybrud.Umbraco.Search.Models.Forms
{
    public class AdValueItem {

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("checked")]
        public bool Checked { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

    }
}