using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionBrowse
{
    public class Torrent
    {
        public uint torrentId { get; set; }
        public uint editionId { get; set; }
        public List<Artist> artists { get; set; }
        public bool remastered { get; set; }
        public uint remasterYear { get; set; }
        public string remasterCatalogueNumber { get; set; }
        public string remasterTitle { get; set; }
        public string media { get; set; }
        public string encoding { get; set; }
        public string format { get; set; }
        public bool hasLog { get; set; }
        public int logScore { get; set; } // Can be negative value
        public bool hasCue { get; set; }
        public bool scene { get; set; }
        public bool vanityHouse { get; set; }
        public uint fileCount { get; set; }
        public DateTime time { get; set; }
        public long size { get; set; }
        public uint snatches { get; set; }
        public uint seeders { get; set; }
        public uint leechers { get; set; }
        public bool isFreeleech { get; set; }
        public bool isNeutralLeech { get; set; }
        public bool isPersonalFreeleech { get; set; }
        public bool canUseToken { get; set; }
        public bool hasSnatched { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", this.encoding, this.format);
        }
    }
}
