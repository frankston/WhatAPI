using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WhatCD.Model.ActionSimilarArtists
{
    public class Artist
    {
        public int id { get; set; }
        public string name { get; set; }
        public int score { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }
}
