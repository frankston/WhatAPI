using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTop10.TypeUsers
{
    public class Response
    {
        public string caption { get; set; }
        public string tag { get; set; }
        public int limit { get; set; }
        public List<Result> results { get; set; }
    }
}
