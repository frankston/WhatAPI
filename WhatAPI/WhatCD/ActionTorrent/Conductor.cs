using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTorrent
{
    public class Conductor
    {
        public int id { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
