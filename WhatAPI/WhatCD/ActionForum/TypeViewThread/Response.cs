using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeViewThread
{
    public class Response
    {
        public uint forumId { get; set; }
        public string forumName { get; set; }
        public uint threadId { get; set; }
        public string threadTitle { get; set; }
        public bool subscribed { get; set; }
        public bool locked { get; set; }
        public bool sticky { get; set; }
        public uint currentPage { get; set; }
        public uint pages { get; set; }
        public Poll poll { get; set; }
        public List<Post> posts { get; set; }

        public override string ToString()
        {
            return this.threadTitle;
        }
    }
}
