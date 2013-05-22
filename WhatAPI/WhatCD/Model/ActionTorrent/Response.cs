using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTorrent
{
    public class Response
    {
        public Group group { get; set; }
        public TorrentInfo torrent { get; set; }
    }
}
