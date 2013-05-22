using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace WhatCD.Model.ActionInbox
{
    public class Response
    {
        public int currentPage { get; set; }
        public int pages { get; set; }
        public List<Message> messages { get; set; }

        public override string ToString()
        {
            return string.Format("Page {0} of {1}", this.currentPage, this.pages);
        }
    }
}
