using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionArtist
{
    public class SimilarArtist
    {
        public int artistId { get; set; }
        public string name { get; set; }
        public int score { get; set; }
        public int similarId { get; set; }
        public override string ToString()
        {
            return this.name;
        }
    }
}
