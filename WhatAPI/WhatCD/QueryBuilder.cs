﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WhatCD
{
    internal class QueryBuilder
    {
        public StringBuilder Query { get; private set; }

        public QueryBuilder(string initialQuery)
        {
            this.Query = new StringBuilder();
            this.Query.Append(initialQuery);
        }

        public void Append(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value)) this.Query.AppendFormat("&{0}={1}", key, HttpUtility.HtmlEncode(value));
        }

        public void Append(string key, int? value)
        {
            if (value != null) this.Query.AppendFormat("&{0}={1}", key, value.ToString());
        }

        public void Append(string key, bool? value)
        {
            if (value != null) this.Query.AppendFormat("&{0}={1}", key, (bool)value ? 1 : 0);
        }

    }
}
