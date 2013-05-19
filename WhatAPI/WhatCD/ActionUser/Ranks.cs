using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionUser
{
    public class Ranks
    {
        public long? uploaded { get; set; }
        public long? downloaded { get; set; }
        public long? uploads { get; set; }
        public uint? requests { get; set; }
        public long? bounty { get; set; }
        public uint? posts { get; set; }
        public uint? artists { get; set; }
        public uint? overall { get; set; }
    }
}
