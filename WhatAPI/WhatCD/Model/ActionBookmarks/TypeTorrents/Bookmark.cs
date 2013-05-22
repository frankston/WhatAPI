using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionBookmarks.TypeTorrents
{
    public class Bookmark
    {
        public int id { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public string recordLabel { get; set; }
        public string catalogueNumber { get; set; }
        public string tagList { get; set; }
        public string releaseType { get; set; }
        public bool vanityHouse { get; set; }
        public string image { get; set; }
        public List<Torrent> torrents { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
