using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTorrent
{
    public class MusicInfo
    {
        public List<Composer> composers { get; set; }
        public List<Dj> dj { get; set; }
        public List<Artist> artists { get; set; }
        public List<With> with { get; set; }
        public List<Conductor> conductor { get; set; }
        public List<RemixedBy> remixedBy { get; set; }
        public List<Producer> producer { get; set; }
    }
}
