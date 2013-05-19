using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTorrentGroup
{
    public class TorrentGroup : IResponse<Response>
    {
        // TODO: Known HTML return issue: https://what.cd/forums.php?action=viewthread&threadid=169772

        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }
    }
}