using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionRequest
{
    public class With
    {
        public uint id { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
