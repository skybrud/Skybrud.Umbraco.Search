using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skybrud.Umbraco.Search.Options.Fields.Conditions {

    public class FieldConditionList : IEnumerable<FieldCondition> {

        private readonly List<FieldCondition> _list;

        #region Properties

        public int Count => _list.Count;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new empty list.
        /// </summary>
        public FieldConditionList() {
            _list = new List<FieldCondition>();
        }

        /// <summary>
        /// Initializes a new list populated with the specified <paramref name="conditions"/>.
        /// </summary>
        /// <param name="conditions">A collection of conditions.</param>
        public FieldConditionList(IEnumerable<FieldCondition> conditions) {
            _list = conditions.ToList();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Adds the specified <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The condition to be added.</param>
        public void Add(FieldCondition condition) {
            _list.Add(condition);
        }

        /// <summary>
        /// Adds the specified <paramref name="conditions"/>.
        /// </summary>
        /// <param name="conditions">The conditions to be added.</param>
        public void AddRange(IEnumerable<FieldCondition> conditions) {
            _list.AddRange(conditions);
        }

        public IEnumerator<FieldCondition> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Operator overloading

        public static implicit operator FieldConditionList(List<FieldCondition> conditions) {
            return new FieldConditionList(conditions);
        }

        public static implicit operator FieldConditionList(FieldCondition[] conditions) {
            return new FieldConditionList(conditions);
        }

        #endregion

    }

}