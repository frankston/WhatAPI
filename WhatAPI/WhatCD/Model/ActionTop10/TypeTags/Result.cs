using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTop10.TypeTags
{
    public class Result
    {
        public string name { get; set; }
        public int uses { get; set; }
        public int posVotes { get; set; }
        public int negVotes { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
