﻿using System;
using System.Web;
using Skybrud.Umbraco.Search.Options.Groups;

namespace Skybrud.Umbraco.Search.Models.Groups {

    public class SearchGroup {

        public int Id { get; }

        public string Name { get; }

        public int Limit { get; }

        public Func<SearchGroup, HttpRequestBase, GroupedSearchOptionsBase, SearchGroupResultList> Callback { get; }

        public SearchGroup(int id, string name, int limit, Func<SearchGroup, HttpRequestBase, GroupedSearchOptionsBase, SearchGroupResultList> callback) {
            Id = id;
            Name = name;
            Limit = limit;
            Callback = callback;
        }

    }

}