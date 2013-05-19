using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionBrowse
{
    public class Artist
    {
        public uint id { get; set; }
        public string name { get; set; }
        public uint aliasid { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
