using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTop10.TypeTorrents
{
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
        public object data { get; set; } // TODO: Unsure what this holds - safest to store as an object until an instance can be found and examined

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.artist, this.groupName);
        }
    }
}
