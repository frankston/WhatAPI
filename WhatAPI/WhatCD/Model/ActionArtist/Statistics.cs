using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionArtist
{
    public class Statistics
    {
        public int? numGroups { get; set; }
        public int? numTorrents { get; set; }
        public int? numSeeders { get; set; }
        public int? numLeechers { get; set; }
        public int? numSnatches { get; set; }
    }
}
