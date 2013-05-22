using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionForum.TypeViewForum
{
    public class Thread
    {
        public int topicId { get; set; }
        public string title { get; set; }
        public int authorId { get; set; }
        public string authorName { get; set; }
        public bool locked { get; set; }
        public bool sticky { get; set; }
        public int postCount { get; set; }
        public int lastID { get; set; }
        public DateTime lastTime { get; set; }
        public int lastAuthorId { get; set; }
        public string lastAuthorName { get; set; }
        public int lastReadPage { get; set; }
        public int lastReadPostId { get; set; }
        public bool read { get; set; }

        public override string ToString()
        {
            return this.title;
        }
    }
}
