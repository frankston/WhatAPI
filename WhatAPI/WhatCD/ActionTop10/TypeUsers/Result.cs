using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTop10.TypeUsers
{
    public class Result
    {
        public uint id { get; set; }
        public string username { get; set; }
        public long uploaded { get; set; }
        public uint upSpeed { get; set; }
        public long downloaded { get; set; }
        public uint downSpeed { get; set; }
        public uint numUploads { get; set; }
        public DateTime joinDate { get; set; }
        // TODO: Thorough unit testing to see if any properties need to be nullable
    }
}
