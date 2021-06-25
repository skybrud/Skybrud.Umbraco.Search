using System;
using System.Collections.Generic;
using System.Linq;

namespace Skybrud.Umbraco.Search.Options {

    public class QueryList {

        private readonly List<object> _list;

        public QueryList() {
            _list = new List<object>();
        }
        
        public virtual string GetRawQuery() {
            return string.Join(" AND ", from item in _list select item.ToString());
        }

        public virtual void Add(object value) {

            switch (value) {

                case null:
                    return;

                case string str:
                    _list.Add(str);
                    break;

                case QueryList list:
                    _list.Add(list);
                    break;

                default:
                    throw new Exception($"Unsupported type: {value.GetType()}");

            }

        }

    }

}