using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTop10.TypeTags
{
    public class Result
    {
        public string name { get; set; }
        public uint uses { get; set; }
        public uint posVotes { get; set; }
        public uint negVotes { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
