using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionBrowse
{
    public class Result
    {
        public uint groupId { get; set; }
        public string groupName { get; set; }
        public string artist { get; set; }
        public List<string> tags { get; set; }
        public bool bookmarked { get; set; }
        public bool vanityHouse { get; set; }
        public uint groupYear { get; set; }
        public string releaseType { get; set; }
        public string groupTime { get; set; }
        public long maxSize { get; set; }
        public uint totalSnatched { get; set; }
        public uint totalSeeders { get; set; }
        public uint totalLeechers { get; set; }
        public List<Torrent> torrents { get; set; }
        public uint? torrentId { get; set; }
        public string category { get; set; }
        public uint fileCount { get; set; }
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
            return string.Format("{0} - {1}", this.artist, this.groupName);
        }
    }
}
