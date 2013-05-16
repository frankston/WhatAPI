using System;
using System.Collections.Generic;

namespace WhatCD
{
    public class ForumCategories
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Category
        {
            public uint categoryID { get; set; }
            public string categoryName { get; set; }
            public List<Forum> forums { get; set; }

            public override string ToString()
            {
                return this.categoryName;
            }
        }

        public class Forum
        {
            public uint forumId { get; set; }
            public string forumName { get; set; }
            public string forumDescription { get; set; }
            public uint numTopics { get; set; }
            public uint numPosts { get; set; }
            public uint lastPostId { get; set; }
            public uint lastAuthorId { get; set; }
            public string lastPostAuthorName { get; set; }
            public uint lastTopicId { get; set; }
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

        public class Response
        {
            public List<Category> categories { get; set; }
        }
    }
}