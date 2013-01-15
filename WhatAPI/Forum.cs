using System;
using System.Collections.Generic;

namespace What
{
    public class Forum
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return status;
        }

        public class Response
        {
            public string forumName { get; set; }
            public List<SpecificRule> specificRules { get; set; }
            public uint currentPage { get; set; }
            public uint pages { get; set; }
            public List<Thread> threads { get; set; }

            public override string ToString()
            {
                return forumName;
            }
        }

        public class SpecificRule
        {
            public uint threadId { get; set; }
            public string thread { get; set; }

            public override string ToString()
            {
                return thread;
            }
        }

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
                return title;
            }
        }
    }
}