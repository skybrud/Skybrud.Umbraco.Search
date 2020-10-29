using System;
using System.Text.RegularExpressions;
using Skybrud.Umbraco.Search.Options.Fields;

namespace Skybrud.Umbraco.Search.Options.Text {

    public class SimpleTextQuery : ITextQuery {

        public FieldList Fields { get; set; }

        public string Text { get; set; }

        public SimpleTextQuery(string text, FieldList fields) {
            Text = text;
            Fields = fields;
        }

        public string GetRawQuery() {

            if (string.IsNullOrWhiteSpace(Text)) return null;

            string text = Regex.Replace(Text, @"[^\wæøåÆØÅ\-@\. ]", string.Empty).ToLowerInvariant().Trim();

            string[] terms = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (terms.Length == 0) return null;

            // fallback if no fields are added
            FieldList fields = Fields == null || Fields.IsEmpty ? FieldList.DefaultFields : Fields;

            return fields.GetQuery(terms);

        }

    }

}