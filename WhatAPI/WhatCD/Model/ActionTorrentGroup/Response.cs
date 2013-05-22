using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTorrentGroup
{
    public class Response
    {
        public Group group { get; set; }
        public List<Torrent> torrents { get; set; }
    }
}
