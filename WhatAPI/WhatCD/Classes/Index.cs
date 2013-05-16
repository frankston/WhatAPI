namespace WhatCD
{
    public class Index
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Notifications
        {
            public uint messages { get; set; }
            public uint notifications { get; set; }
            public bool newAnnouncement { get; set; }
            public bool newBlog { get; set; }
        }

        public class Response
        {
            public string username { get; set; }
            public uint id { get; set; }
            public string authkey { get; set; }
            public string passkey { get; set; }
            public Notifications notifications { get; set; }
            public Userstats userstats { get; set; }

            public override string ToString()
            {
                return this.username;
            }
        }

        public class Userstats
        {
            public long uploaded { get; set; }
            public long downloaded { get; set; }
            public double ratio { get; set; }
            public double requiredratio { get; set; }
            public string @class { get; set; }

            public override string ToString()
            {
                return this.@class;
            }
        }
    }
}