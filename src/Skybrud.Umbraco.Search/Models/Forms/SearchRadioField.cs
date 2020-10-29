using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using Skybrud.Essentials.Reflection;
using Skybrud.Essentials.Strings.Extensions;

namespace Skybrud.Umbraco.Search.Models.Forms {

    public class SearchRadioField : SearchFieldBase {

        [JsonProperty("value", Order = 500)]
        public new List<SearchRadioValue> Value { get; }

        public SearchRadioField(string name) : base(SearchFieldType.Radio, name) {
            Value = new List<SearchRadioValue>();
        }

        public SearchRadioField(string name, IEnumerable<SearchRadioValue> value) : base(SearchFieldType.Radio, name) {
            Value = value?.ToList() ?? new List<SearchRadioValue>();
        }

        public SearchRadioField(string name, params SearchRadioValue[] value) : base(SearchFieldType.Radio, name) {
            Value = value?.ToList() ?? new List<SearchRadioValue>();
        }

        public SearchRadioField(string name, string label) : base(SearchFieldType.Radio, name) {
            Label = label;
            Value = new List<SearchRadioValue>();
        }

        public SearchRadioField(string name, string label, IEnumerable<SearchRadioValue> value) : base(SearchFieldType.Radio, name) {
            Label = label;
            Value = value?.ToList() ?? new List<SearchRadioValue>();
        }

        public SearchRadioField(string name, string label, params SearchRadioValue[] value) : base(SearchFieldType.Radio, name) {
            Label = label;
            Value = value?.ToList() ?? new List<SearchRadioValue>();
        }

        public static SearchRadioField CreateFromEnum<T>(string name, string label) where T : Enum {
            return CreateFromEnum<T>(name, label, default);
        }

        public static SearchRadioField CreateFromEnum<T>(string name, string label, T defaultValue) where T : Enum {

            List<SearchRadioValue> values = new List<SearchRadioValue>();

            foreach (T value in (T[]) Enum.GetValues(typeof(T))) {

                string valueName = value.ToString();

                if (ReflectionUtils.HasCustomAttribute(value, out DescriptionAttribute result)) {
                    valueName = result.Description;
                }

                values.Add(new SearchRadioValue(value.ToCamelCase(), valueName, Equals(value, defaultValue)));

            }

            return new SearchRadioField(name, label, values);

        }

    }

}