using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Examine;
using Examine.Providers;
using Examine.SearchCriteria;

namespace Skybrud.Umbraco.Search.Models.Options
{
    public class Order
    {
        #region Properties

        public string FieldName { get; set; }
        public OrderType OrderType { get; set; }
        public OrderDirection OrderDirection { get; set; }

        //public bool IsValid => !string.IsNullOrWhiteSpace(FieldName);

        #endregion

        #region Constructors

        public Order()
        {
            FieldName = "createDate";
            OrderType = OrderType.String;
            OrderDirection = OrderDirection.Descending;
        }

        private Order(string fieldName)
        {
            FieldName = fieldName;
            OrderType = OrderType.String;
            OrderDirection = OrderDirection.Descending;
        }

        private Order(string fieldName, OrderType orderType)
        {
            FieldName = fieldName;
            OrderType = orderType;
            OrderDirection = OrderDirection.Descending;
        }

        private Order(string fieldName, OrderType orderType, OrderDirection orderDirection)
        {
            FieldName = fieldName;
            OrderType = orderType;
            OrderDirection = orderDirection;
        }

        #endregion

        #region Static Methods

        public static Order GetOrderOptions()
        {
            return new Order();
        }

        public static Order GetOrderOptions(string fieldName)
        {
            return new Order(fieldName);
        }
        public static Order GetOrderOptions(string fieldName, OrderType orderType)
        {
            return new Order(fieldName, orderType);
        }

        public static Order GetOrderOptions(string fieldName, OrderType orderType, OrderDirection orderDirection)
        {
            return new Order(fieldName, orderType, orderDirection);
        }

        #endregion

        #region Members

        public virtual IOrderedEnumerable<SearchResult> OrderResults(BaseSearchProvider externalSearcher,ISearchCriteria criteria)
        {
            IOrderedEnumerable<SearchResult> items;
            switch (OrderType)
            {
                case OrderType.Score:
                    items = OrderDirection == OrderDirection.Descending
                        ? externalSearcher.Search(criteria)
                            .OrderByDescending(o => o.Score)
                        : externalSearcher.Search(criteria)
                            .OrderBy(o => o.Score);
                    break;
                case OrderType.Int:
                    items = OrderDirection == OrderDirection.Descending
                        ? externalSearcher.Search(criteria).Where(x => x.Fields.ContainsKey(FieldName))
                            .OrderByDescending(o => Convert.ToInt32(o[FieldName]))
                        : externalSearcher.Search(criteria).Where(x => x.Fields.ContainsKey(FieldName))
                            .OrderBy(o => Convert.ToInt32(o[FieldName]));
                    break;
                default:
                    items = OrderDirection == OrderDirection.Descending
                        ? externalSearcher.Search(criteria)
                            .OrderByDescending(o => o.Fields.ContainsKey(FieldName) ? o.Fields[FieldName] : o.Fields["createDate"])
                        : externalSearcher.Search(criteria)
                            .OrderBy(o => o.Fields.ContainsKey(FieldName) ? o.Fields[FieldName] : o.Fields["createDate"]);
                    break;
            }

            return  items;
        }
        public static IOrderedEnumerable<SearchResult> OrderResultSet(List<SearchResult> items, Order order)
        {
            IOrderedEnumerable<SearchResult> orderedItems;
            switch (order.OrderType)
            {
                case OrderType.Score:
                    orderedItems = order.OrderDirection == OrderDirection.Descending
                        ? items.OrderByDescending(o => o.Score)
                        : items.OrderBy(o => o.Score);
                    break;
                case OrderType.Int:
                    orderedItems = order.OrderDirection == OrderDirection.Descending
                        ? items.Where(x => x.Fields.ContainsKey(order.FieldName)).OrderByDescending(o => Convert.ToInt32(order.FieldName))
                        : items.Where(x => x.Fields.ContainsKey(order.FieldName)).OrderBy(o => Convert.ToInt32(o.Fields[order.FieldName]));
                    break;
                default:
                    orderedItems = order.OrderDirection == OrderDirection.Descending
                        ? items
                            .OrderByDescending(o => o.Fields.ContainsKey(order.FieldName) ? o.Fields[order.FieldName] : o.Fields["createDate"])
                        : items
                            .OrderBy(o => o.Fields.ContainsKey(order.FieldName) ? o.Fields[order.FieldName] : o.Fields["createDate"]);
                    break;
            }

            return orderedItems;
        }
        #endregion

    }

    public enum OrderType
    {
        [Description("Order results alphabetically (default)")]
        String,
        [Description("Order results numerically")]
        Int,
        [Description("Order results by score")]
        Score
    }
    public enum OrderDirection
    {
        Ascending,
        Descending
    }
}
