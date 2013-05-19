using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionArtist
{
    public class Tag
    {
        public string name { get; set; }
        public uint count { get; set; }
        public override string ToString()
        {
            return this.name;
        }
    }
}
