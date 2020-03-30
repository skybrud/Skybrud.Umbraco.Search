using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skybrud.Umbraco.Search.Options.Groups {

    /// <summary>
    /// List of <see cref="ISearchGroup"/>.
    /// </summary>
    public class SearchGroupList : IEnumerable<ISearchGroup> {

        private readonly List<ISearchGroup> _list;

        #region Constructors

        /// <summary>
        /// Initializes an empty list.
        /// </summary>
        public SearchGroupList() {
            _list = new List<ISearchGroup>();
        }

        /// <summary>
        /// Initializes a new list with the specified <paramref name="groups"/>.
        /// </summary>
        /// <param name="groups">The groups that should be added to the list.</param>
        public SearchGroupList(IEnumerable<ISearchGroup> groups) {
            _list = groups.ToList();
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Adds the specified <paramref name="group"/>.
        /// </summary>
        /// <param name="group">The group to be added.</param>
        public void Add(ISearchGroup group) {
            _list.Add(group);
        }

        /// <summary>
        /// Adds the specified collection of <paramref name="groups"/> to the list.
        /// </summary>
        /// <param name="groups"></param>
        public void AddRange(IEnumerable<ISearchGroup> groups) {
            _list.AddRange(groups);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the underlying <see cref="List{ISearchGroup}"/>.
        /// </summary>
        /// <returns>A <see cref="List{ISearchGroup}.Enumerator"/> for the underlying <see cref="List{ISearchGroup}"/>.</returns>
        public IEnumerator<ISearchGroup> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

        #region Operator overloading

        public static implicit operator SearchGroupList(List<ISearchGroup> groups) {
            return new SearchGroupList(groups);
        }

        public static implicit operator SearchGroupList(ISearchGroup[] groups) {
            return new SearchGroupList(groups);
        }

        #endregion

    }

}