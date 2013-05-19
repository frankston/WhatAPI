using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionBrowse
{
    public class Response
    {
        public uint currentPage { get; set; }
        public uint pages { get; set; }
        public List<Result> results { get; set; }

        public override string ToString()
        {
            return string.Format("Page {0} of {1}", this.currentPage, this.pages);
        }
    }
}
