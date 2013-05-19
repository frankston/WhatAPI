using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionNotifications
{
    public class Response
    {
        public uint currentPages { get; set; }
        public uint pages { get; set; }
        public uint numNew { get; set; }
        public List<Result> results { get; set; } // TODO: Make note of why IResponse cannot be implemented here

        public override string ToString()
        {
            return string.Format("Page {0} of {1}", this.currentPages, this.pages);
        }
    }
}
