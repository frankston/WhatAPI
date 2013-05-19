using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionAnnouncements
{
    public class BlogPost
    {
        public uint blogId { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public DateTime blogTime { get; set; }
        public uint threadId { get; set; }

        public override string ToString()
        {
            return this.title;
        }
    }
}
