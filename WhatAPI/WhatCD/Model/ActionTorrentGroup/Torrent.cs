using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTorrentGroup
{
    public class Torrent
    {
        public int id { get; set; }
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
        public int logScore { get; set; } // Note: Can be negative value
        public int fileCount { get; set; }
        public long size { get; set; }
        public int seeders { get; set; }
        public int leechers { get; set; }
        public int snatched { get; set; }
        public bool freeTorrent { get; set; }
        public DateTime time { get; set; }
        public string description { get; set; }
        public string fileList { get; set; }
        public string filePath { get; set; }
        public int userId { get; set; }
        public string username { get; set; }

        public override string ToString()
        {
            return this.filePath;
        }
    }
}
