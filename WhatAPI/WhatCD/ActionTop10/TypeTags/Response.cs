using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTop10.TypeTags
{
    public class Response
    {
        public string caption { get; set; }
        public string tag { get; set; }
        public uint limit { get; set; }
        public List<Result> results { get; set; }

        public override string ToString()
        {
            return this.caption;
        }
    }
}
