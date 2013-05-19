using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionUser
{
    public class Response
    {
        public string username { get; set; }
        public string avatar { get; set; }
        public bool isFriend { get; set; }
        public string profileText { get; set; }
        public Stats stats { get; set; }
        public Ranks ranks { get; set; }
        public Personal personal { get; set; }
        public Community community { get; set; }

        public override string ToString()
        {
            return this.username;
        }
    }
}
