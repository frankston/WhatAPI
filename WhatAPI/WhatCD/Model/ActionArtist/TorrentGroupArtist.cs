using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WhatCD.Model.ActionArtist
{
    [JsonObject("Artist")]
    public class TorrentGroupArtist
    {
        public int id { get; set; }
        public string name { get; set; }
        public int aliasid { get; set; }
    }
}
