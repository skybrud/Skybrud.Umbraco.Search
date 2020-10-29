using System;
using Newtonsoft.Json;

namespace Skybrud.Umbraco.Search.Models.Forms{
    public class SearchRadioValue {

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("checked")]
        public bool IsChecked { get; set; }

        public SearchRadioValue(int value, string label) {
            Value = value.ToString();
            Label = label;
        }

        public SearchRadioValue(int value, string label, bool isChecked) {
            Value = value.ToString();
            Label = label;
            IsChecked = isChecked;
        }

        public SearchRadioValue(Guid value, string label) {
            Value = value.ToString();
            Label = label;
        }

        public SearchRadioValue(Guid value, string label, bool isChecked) {
            Value = value.ToString();
            Label = label;
            IsChecked = isChecked;
        }

        public SearchRadioValue(string value, string label) {
            Value = value;
            Label = label;
        }

        public SearchRadioValue(string value, string label, bool isChecked) {
            Value = value;
            Label = label;
            IsChecked = isChecked;
        }
    }
}