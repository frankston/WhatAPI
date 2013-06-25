using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionUserSearch
{
    public class Result
    {
        public int userId { get; set; }
        public string username { get; set; }
        public bool donor { get; set; }
        public bool warned { get; set; }
        public bool enabled { get; set; }
        public string @class { get; set; }
        public string avatar { get; set; }

        public override string ToString()
        {
            return this.username;
        }
    }
}
