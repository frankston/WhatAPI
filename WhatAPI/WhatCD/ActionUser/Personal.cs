using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionUser
{
    public class Personal
    {
        public string @class { get; set; }
        public uint paranoia { get; set; }
        public string paranoiaText { get; set; }
        public bool donor { get; set; }
        public bool warned { get; set; }
        public bool enabled { get; set; }
        public string passkey { get; set; }

        public override string ToString()
        {
            return this.@class;
        }
    }
}
