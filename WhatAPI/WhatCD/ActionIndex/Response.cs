using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionIndex
{
    public class Response
    {
        public string username { get; set; }
        public uint id { get; set; }
        public string authkey { get; set; }
        public string passkey { get; set; }
        public Notifications notifications { get; set; }
        public Userstats userstats { get; set; }

        public override string ToString()
        {
            return this.username;
        }
    }
}
