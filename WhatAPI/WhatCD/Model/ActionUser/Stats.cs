using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionUser
{
    public class Stats
    {
        public DateTime joinedDate { get; set; }
        public DateTime? lastAccess { get; set; }
        public long? uploaded { get; set; }
        public long? downloaded { get; set; }
        public double? ratio { get; set; }
        public double? requiredRatio { get; set; }
    }
}
