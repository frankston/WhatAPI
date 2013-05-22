using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionNotifications
{
    public class Response
    {
        public int currentPages { get; set; }
        public int pages { get; set; }
        public int numNew { get; set; }
        public List<Result> results { get; set; }

        public override string ToString()
        {
            return string.Format("Page {0} of {1}", this.currentPages, this.pages);
        }
    }
}
