using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeViewForum
{
    public class Response
    {
        public string forumName { get; set; }
        public List<SpecificRule> specificRules { get; set; }
        public uint currentPage { get; set; }
        public uint pages { get; set; }
        public List<Thread> threads { get; set; }

        public override string ToString()
        {
            return this.forumName;
        }
    }
}
