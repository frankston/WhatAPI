using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionRequests
{
    public class Artist
    {
        public string id { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
