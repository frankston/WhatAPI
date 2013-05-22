using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionForum.TypeMain
{
    public class Forum
    {
        public int forumId { get; set; }
        public string forumName { get; set; }
        public string forumDescription { get; set; }
        public int numTopics { get; set; }
        public int numPosts { get; set; }
        public int lastPostId { get; set; }
        public int lastAuthorId { get; set; }
        public string lastPostAuthorName { get; set; }
        public int lastTopicId { get; set; }
        public DateTime lastTime { get; set; }
        public List<uint> specificRules { get; set; }
        public string lastTopic { get; set; }
        public bool read { get; set; }
        public bool locked { get; set; }
        public bool sticky { get; set; }

        public override string ToString()
        {
            return this.forumName;
        }
    }
}
