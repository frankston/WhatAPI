using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionInbox.TypeViewConv
{
    public class InboxViewConv : IResponse<Response>
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }
    }
}