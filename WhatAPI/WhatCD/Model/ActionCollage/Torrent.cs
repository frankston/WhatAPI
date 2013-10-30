using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatCD.Model.ActionCollage
{
    public class Torrent
    {
        public int torrentid { get; set; }
        public string media { get; set; }
        public string format { get; set; }
        public string encoding { get; set; }
        public bool remastered { get; set; }
        public int remasterYear { get; set; }
        public string remasterTitle { get; set; }
        public string remasterRecordLabel { get; set; }
        public string remasterCatalogueNumber { get; set; }
        public bool scene { get; set; }
        public bool hasLog { get; set; }
        public bool hasCue { get; set; }
        public int logScore { get; set; }
        public int fileCount { get; set; }
        public long size { get; set; }
        public int seeders { get; set; }
        public int leechers { get; set; }
        public int snatched { get; set; }
        public bool freeTorrent { get; set; }
        public bool reported { get; set; }
        public string time { get; set; }
    }
}
