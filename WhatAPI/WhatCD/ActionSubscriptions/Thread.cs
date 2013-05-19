using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionSubscriptions
{
    public class Thread
    {
        public uint forumId { get; set; }
        public string forumName { get; set; }
        public uint threadId { get; set; }
        public string threadTitle { get; set; }
        public uint postId { get; set; }
        public uint lastPostId { get; set; }
        public bool locked { get; set; }
        public bool @new { get; set; }

        public override string ToString()
        {
            return this.threadTitle;
        }
    }
}
