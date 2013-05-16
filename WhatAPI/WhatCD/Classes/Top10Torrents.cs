using System.Collections.Generic;

namespace WhatCD
{
    public class Top10Torrents
    {
        public string status { get; set; }
        public List<Response> response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Response
        {
            public string caption { get; set; }
            public string tag { get; set; }
            public uint limit { get; set; }
            public List<Result> results { get; set; }

            public override string ToString()
            {
                return this.caption;
            }
        }

        public class Result
        {
            public uint torrentId { get; set; }
            public uint groupId { get; set; }
            public string artist { get; set; }
            public string groupName { get; set; }
            public uint groupCategory { get; set; }
            public uint groupYear { get; set; }
            public string remasterTitle { get; set; }
            public string format { get; set; }
            public string encoding { get; set; }
            public bool hasLog { get; set; }
            public bool hasCue { get; set; }
            public string media { get; set; }
            public bool scene { get; set; }
            public uint year { get; set; }
            public List<string> tags { get; set; }
            public uint snatched { get; set; }
            public uint seeders { get; set; }
            public uint leechers { get; set; }
            public object data { get; set; } // Unsure what this holds - safest to store as an object until an instance can be found and examined
            
            public override string ToString()
            {
                return string.Format("{0} - {1}", this.artist, this.groupName);
            }
        }
    }
}