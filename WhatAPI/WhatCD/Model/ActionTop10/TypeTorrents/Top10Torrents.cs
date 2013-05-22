using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionTop10.TypeTorrents
{
    public class Top10Torrents : IResponse<List<Response>>
    {
        public string status { get; set; }
        public List<Response> response { get; set; }

        public override string ToString()
        {
            return this.status;
        }
    }
}