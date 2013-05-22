﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionBookmarks.TypeTorrents
{
    public class Torrent
    {
        public int id { get; set; }
        public int groupId { get; set; }
        public string media { get; set; }
        public string format { get; set; }
        public string encoding { get; set; }
        public int remasterYear { get; set; }
        public bool remastered { get; set; }
        public string remasterTitle { get; set; }
        public string remasterRecordLabel { get; set; }
        public string remasterCatalogueNumber { get; set; }
        public bool scene { get; set; }
        public bool hasLog { get; set; }
        public bool hasCue { get; set; }
        public int logScore { get; set; } // Can be negative value
        public int fileCount { get; set; }
        public bool freeTorrent { get; set; }
        public long size { get; set; }
        public int leechers { get; set; }
        public int seeders { get; set; }
        public int snatched { get; set; }
        public DateTime time { get; set; }
        public int hasFile { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.encoding, this.format);
        }
    }
}