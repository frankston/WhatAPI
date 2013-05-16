using System.Collections.Generic;

namespace WhatCD
{
    public class Subscriptions
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Response
        {
            public List<Thread> threads { get; set; }
        }

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
}