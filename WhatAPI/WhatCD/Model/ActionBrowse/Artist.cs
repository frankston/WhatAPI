using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionBrowse
{
    public class Artist
    {
        public int id { get; set; }
        public string name { get; set; }
        public int aliasid { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
