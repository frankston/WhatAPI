using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeViewThread
{
    public class Author
    {
        public uint authorId { get; set; }
        public string authorName { get; set; }
        public List<string> paranoia { get; set; }
        public bool artist { get; set; }
        public bool donor { get; set; }
        public bool warned { get; set; }
        public string avatar { get; set; }
        public bool enabled { get; set; }
        public string userTitle { get; set; }

        public override string ToString()
        {
            return this.authorName;
        }
    }
}
