using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatCD.Model.ActionCollage
{
    public class Torrentgroup
    {
        public string id { get; set; }
        public string name { get; set; }
        public string year { get; set; }
        public string categoryId { get; set; }
        public string recordLabel { get; set; }
        public string catalogueNumber { get; set; }
        public string vanityHouse { get; set; }
        public string tagList { get; set; }
        public string releaseType { get; set; }
        public string wikiImage { get; set; }
        public MusicInfo musicInfo { get; set; }
        public List<Torrent> torrents { get; set; }
    }
}
