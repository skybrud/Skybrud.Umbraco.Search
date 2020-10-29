using System.Collections.Generic;
using Newtonsoft.Json;
using Skybrud.Umbraco.Search.Models.Forms.Input;

namespace Skybrud.Umbraco.Search.Models.Forms {

    public class SearchFormModel {

        #region Properties

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("endpointUrl")]
        public string EndpointUrl { get; set; }

        [JsonProperty("fields")]
        public List<SearchFieldBase> Fields { get; set; }

        #endregion

        #region Constructors

        public SearchFormModel() { }

        public SearchFormModel(string endpointUrl) {
            EndpointUrl = endpointUrl;
            Fields = new List<SearchFieldBase>();
        }

        #endregion

        #region Member methods

        public SearchFormModel SetTitle(string title) {
            Title = title;
            return this;
        }

        public SearchFormModel AddField(SearchFieldBase field) {
            Fields.Add(field);
            return this;
        }

        public SearchFormModel AddField(SearchFieldType type, string name, object value) {
            Fields.Add(new SearchFieldBase(type, name, value));
            return this;
        }

        public SearchFormModel AddHiddenField(string name, object value) {
            Fields.Add(new SearchFieldBase(SearchFieldType.Hidden, name, value));
            return this;
        }

        public static SearchFormModel New(string endpointUrl) {
            return new SearchFormModel(endpointUrl);
        }

        public SearchFormModel AddEmailField(string name, string value = null, string placeholder = null, string label = null, bool required = false) {
            Fields.Add(new EmailInputField {
                Name = name,
                Value = value,
                Placeholder = placeholder,
                Label = label,
                IsRequired = required
            });
            return this;
        }

        public SearchFormModel AddTextField(string name, string value = null, string placeholder = null, string label = null, string pattern = null, int? size = null, bool required = false) {
            Fields.Add(new TextInputField {
                Name = name,
                Value = value,
                Placeholder = placeholder,
                Label = label,
                Pattern = pattern,
                Size = size,
                IsRequired = required
            });
            return this;
        }

        public SearchFormModel AddTextarea(string name, string value = null, string placeholder = null, string label = null, string pattern = null, int? size = null, bool required = false) {
            Fields.Add(new Textarea {
                Name = name,
                Value = value,
                Placeholder = placeholder,
                Label = label,
                Pattern = pattern,
                Size = size,
                IsRequired = required
            });
            return this;
        }

        public SearchFormModel AddTelField(string name, string value = null, string placeholder = null, string label = null, string pattern = null, bool required = false) {
            Fields.Add(new TelInputField {
                Name = name,
                Value = value,
                Placeholder = placeholder,
                Label = label,
                Pattern = pattern,
                IsRequired = required
            });
            return this;
        }

        public SearchFormModel AddNumberField(string name, string value = null, string placeholder = null,
            string label = null, string pattern = null, int? min = null, int? max = null, int? size = null, bool required = false) {

            Fields.Add(new NumberInputField {
                Name = name,
                Value = value,
                Placeholder = placeholder,
                Label = label,
                Pattern = pattern,
                Min = min,
                Max = max,
                Size = size,
                IsRequired = required
            });

            return this;

        }

        public SearchFormModel AddRadioGroup(string name, IEnumerable<SearchRadioValue> value) {
            Fields.Add(new SearchRadioField(name, value));
            return this;
        }

        public SearchFormModel AddRadioGroup(string name, params SearchRadioValue[] value) {
            Fields.Add(new SearchRadioField(name, value));
            return this;
        }

        public SearchFormModel AddRadioGroup(string name, string label, IEnumerable<SearchRadioValue> value) {
            Fields.Add(new SearchRadioField(name, label, value));
            return this;
        }

        public SearchFormModel AddRadioGroup(string name, string label, params SearchRadioValue[] value) {
            Fields.Add(new SearchRadioField(name, label, value));
            return this;
        }

        public SearchFormModel SetUrl(string url) {
            Url = url;
            return this;
        }

        public SearchFormModel AddCheckbox(string name, string value = null, string label = null, bool required = false, string description = null) {
            Fields.Add(new SearchFieldBase(SearchFieldType.Checkbox, name, value)  {
                Label = label,
                IsRequired = required,
                Description = description
            });
            return this;
        }

        public SearchFormModel AddButton(string label) {
            Fields.Add(new Button { Label = label });
            return this;
        }

        public SearchFormModel AddSubmitButton(string label) {
            Fields.Add(new SubmitButton { Label = label });
            return this;
        }

        #endregion

    }

}