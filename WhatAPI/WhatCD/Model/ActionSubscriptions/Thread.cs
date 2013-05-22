using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionSubscriptions
{
    public class Thread
    {
        public int forumId { get; set; }
        public string forumName { get; set; }
        public int threadId { get; set; }
        public string threadTitle { get; set; }
        public int postId { get; set; }
        public int lastPostId { get; set; }
        public bool locked { get; set; }
        public bool @new { get; set; }

        public override string ToString()
        {
            return this.threadTitle;
        }
    }
}
