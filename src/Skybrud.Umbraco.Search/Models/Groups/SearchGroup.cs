using System;
using System.Web;

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class SearchGroup {

        public int Id { get; }

        public string Name { get; }

        public int Limit { get; }

        public Func<SearchGroup, HttpRequestBase, SearchGroupResultList> Callback { get; }

        public SearchGroup(int id, string name, int limit, Func<SearchGroup, HttpRequestBase, SearchGroupResultList> callback) {
            Id = id;
            Name = name;
            Limit = limit;
            Callback = callback;
        }

    }

}