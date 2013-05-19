using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeViewForum
{
    public class Thread
    {
        public uint topicId { get; set; }
        public string title { get; set; }
        public uint authorId { get; set; }
        public string authorName { get; set; }
        public bool locked { get; set; }
        public bool sticky { get; set; }
        public uint postCount { get; set; }
        public uint lastID { get; set; }
        public DateTime lastTime { get; set; }
        public uint lastAuthorId { get; set; }
        public string lastAuthorName { get; set; }
        public uint lastReadPage { get; set; }
        public uint lastReadPostId { get; set; }
        public bool read { get; set; }

        public override string ToString()
        {
            return this.title;
        }
    }
}
