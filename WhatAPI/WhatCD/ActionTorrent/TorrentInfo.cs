using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WhatCD.ActionTorrent
{
    [JsonObject("Torrent")]
    public class TorrentInfo
    {
        public uint id { get; set; }
        public string media { get; set; }
        public string format { get; set; }
        public string encoding { get; set; }
        public bool remastered { get; set; }
        public uint remasterYear { get; set; }
        public string remasterTitle { get; set; }
        public string remasterRecordLabel { get; set; }
        public string remasterCatalogueNumber { get; set; }
        public bool scene { get; set; }
        public bool hasLog { get; set; }
        public bool hasCue { get; set; }
        public int logScore { get; set; }
        public uint fileCount { get; set; }
        public long size { get; set; }
        public uint seeders { get; set; }
        public uint leechers { get; set; }
        public uint snatched { get; set; }
        public bool freeTorrent { get; set; }
        public string time { get; set; }
        public string description { get; set; }
        public string fileList { get; set; }
        public string filePath { get; set; }
        public uint userId { get; set; }
        public string username { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.encoding, this.format);
        }
    }
}
