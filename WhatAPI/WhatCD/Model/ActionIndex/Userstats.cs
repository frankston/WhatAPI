using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionIndex
{
    public class Userstats
    {
        public long uploaded { get; set; }
        public long downloaded { get; set; }
        public double ratio { get; set; }
        public double requiredratio { get; set; }
        public string @class { get; set; }

        public override string ToString()
        {
            return this.@class;
        }
    }
}
