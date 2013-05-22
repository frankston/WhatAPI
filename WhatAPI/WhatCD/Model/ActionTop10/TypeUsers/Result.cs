using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTop10.TypeUsers
{
    public class Result
    {
        public int id { get; set; }
        public string username { get; set; }
        public double uploaded { get; set; }
        public double upSpeed { get; set; }
        public double downloaded { get; set; }
        public double downSpeed { get; set; }
        public int numUploads { get; set; }
        public DateTime joinDate { get; set; }
    }
}
