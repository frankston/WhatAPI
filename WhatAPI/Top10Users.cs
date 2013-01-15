using System;
using System.Collections.Generic;

namespace What
{
    public class Top10Users
    {
        public string status { get; set; }
        public List<Response> response { get; set; }

        public override string ToString()
        {
            return status;
        }

        public class Response
        {
            public string caption { get; set; }
            public string tag { get; set; }
            public uint limit { get; set; }
            public List<Result> results { get; set; }
        }

        public class Result
        {
            public uint id { get; set; }
            public string username { get; set; }
            public long uploaded { get; set; }
            public uint upSpeed { get; set; }
            public long downloaded { get; set; }
            public uint downSpeed { get; set; }
            public uint numUploads { get; set; }
            public DateTime joinDate { get; set; }
        }
    }
}