using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionArtist
{
    public class Response
    {
        public int id { get; set; }
        public string name { get; set; }
        public bool notificationsEnabled { get; set; }
        public bool hasBookmarked { get; set; }
        public string image { get; set; }
        public string body { get; set; }
        public bool vanityHouse { get; set; }
        public List<Tag> tags { get; set; }
        public List<SimilarArtist> similarArtists { get; set; }
        public Statistics statistics { get; set; }
        public List<TorrentGroup> torrentGroup { get; set; }
        public List<Request> requests { get; set; }
        public override string ToString()
        {
            return this.name;
        }
    }
}
