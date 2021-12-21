using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Skybrud.Umbraco.Search.Options {

    public class ContentTypeList : IEnumerable<string> {

        private readonly List<string> _contentTypes;

        #region Properties

        public int Count => _contentTypes.Count;

        #endregion

        #region Constructors

        public ContentTypeList() {
            _contentTypes = new List<string>();
        }

        public ContentTypeList(IEnumerable<string> contentTypes) {
            _contentTypes = contentTypes.ToList();
        }

        #endregion

        #region Member methods

        public void Add(string contentType) {
            _contentTypes.Add(contentType);
        }

        public void AddRange(params string[] contentTypes) {
            _contentTypes.AddRange(contentTypes);
        }

        public void AddRange(IEnumerable<string> contentTypes) {
            _contentTypes.AddRange(contentTypes);
        }

        public IEnumerator<string> GetEnumerator() {
            return _contentTypes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion

    }

}