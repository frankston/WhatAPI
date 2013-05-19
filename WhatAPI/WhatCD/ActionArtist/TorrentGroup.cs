using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionArtist
{
    public class TorrentGroup
    {
        public uint groupId { get; set; }
        public string groupName { get; set; }
        public uint groupYear { get; set; }
        public string groupRecordLabel { get; set; }
        public string groupCatalogueNumber { get; set; }
        public List<string> tags { get; set; }
        public ReleaseType releaseType { get; set; }
        public bool groupVanityHouse { get; set; }
        public bool hasBookmarked { get; set; }
        public List<Torrent> torrent { get; set; }
        public override string ToString()
        {
            return this.groupName;
        }
    }
}
