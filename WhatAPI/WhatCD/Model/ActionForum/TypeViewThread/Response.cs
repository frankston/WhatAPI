using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionForum.TypeViewThread
{
    public class Response
    {
        public int forumId { get; set; }
        public string forumName { get; set; }
        public int threadId { get; set; }
        public string threadTitle { get; set; }
        public bool subscribed { get; set; }
        public bool locked { get; set; }
        public bool sticky { get; set; }
        public int currentPage { get; set; }
        public int pages { get; set; }
        public Poll poll { get; set; }
        public List<Post> posts { get; set; }

        public override string ToString()
        {
            return this.threadTitle;
        }
    }
}
