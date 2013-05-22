using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTorrentGroup
{
    public class Group
    {
        public string wikiBody { get; set; }
        public string wikiImage { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public string recordLabel { get; set; }
        public string catalogueNumber { get; set; }
        public ReleaseType releaseType { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public DateTime time { get; set; }
        public bool vanityHouse { get; set; }
        public MusicInfo musicInfo { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
