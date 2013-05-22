using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTop10.TypeTorrents
{
    public class Result
    {
        public int torrentId { get; set; }
        public int groupId { get; set; }
        public string artist { get; set; }
        public string groupName { get; set; }
        public int groupCategory { get; set; }
        public int groupYear { get; set; }
        public string remasterTitle { get; set; }
        public string format { get; set; }
        public string encoding { get; set; }
        public bool hasLog { get; set; }
        public bool hasCue { get; set; }
        public string media { get; set; }
        public bool scene { get; set; }
        public int year { get; set; }
        public List<string> tags { get; set; }
        public int snatched { get; set; }
        public int seeders { get; set; }
        public int leechers { get; set; }
        public long data { get; set; }
        public long size { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.artist, this.groupName);
        }
    }
}
