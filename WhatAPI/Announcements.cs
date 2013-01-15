using System;
using System.Collections.Generic;

namespace What
{
    public class Announcements
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return status;
        }

        public class Announcement
        {
            public uint newsId { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public DateTime newsTime { get; set; }

            public override string ToString()
            {
                return title;
            }
        }

        public class BlogPost
        {
            public uint blogId { get; set; }
            public string author { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public DateTime blogTime { get; set; }
            public uint threadId { get; set; }

            public override string ToString()
            {
                return title;
            }
        }

        public class Response
        {
            public List<Announcement> announcements { get; set; }
            public List<BlogPost> blogPosts { get; set; }
        }
    }
}