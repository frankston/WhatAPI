using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionBookmarks.TypeArtists
{
    public class Artist
    {
        public int artistId { get; set; }
        public string artistName { get; set; }

        public override string ToString()
        {
            return this.artistName;
        }
    }
}
