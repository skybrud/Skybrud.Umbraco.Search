using System;

namespace Skybrud.Umbraco.Search.Models.Options
{
    public class DateRange
    {
        #region Properties

        private const string DateFormat = "yyyyMMddHHmm00000";
        public string FieldName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        // IsValid assumes that a daterange search where Start is DateTime.MinValue and
        // End is DateTime.MaxValue or MinValue is equal to not making a daterange search at all
        public bool IsValid => !string.IsNullOrWhiteSpace(FieldName) && (Start != DateTime.MinValue && (End != DateTime.MaxValue || End != DateTime.MinValue));
        public string FormattedStartString => Start != DateTime.MinValue ? Start.ToString(DateFormat) : "19000101000000000";

        public string FormattedEndString => End != DateTime.MinValue ? End.ToString(DateFormat) : "99999999999999999";

        #endregion

        #region Constructors

        public DateRange()
        {
            FieldName = "createDate";
            Start = DateTime.MinValue;
            End = DateTime.MaxValue;
        }

        private DateRange(string fieldName)
        {
            FieldName = fieldName;
            Start = DateTime.MinValue;
            End = DateTime.MaxValue;
        }

        private DateRange(string fieldName, DateTime start, DateTime end)
        {
            FieldName = fieldName;
            Start = start;
            End = end;
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Sets the Start and End properties to DateTime Min and Max, respectively
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DateRange GetFromFieldName(string fieldName)
        {
            return new DateRange(fieldName);
        }

        public static DateRange GetDateRangeOptions(string fieldName, DateTime start, DateTime end)
        {
            return new DateRange(fieldName, start, end);
        }

        #endregion

        #region Members

        public virtual string GetQuery()
        {
            if (IsValid)
            {
                return string.Format(
                    " +({0}:[{1} TO {2}])",
                    FieldName,
                    FormattedStartString,
                    FormattedEndString);
            }
            return "";
        }

        #endregion

    }
}
