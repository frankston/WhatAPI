﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace WhatCD.ActionInbox
{
    public class Response
    {
        public uint currentPage { get; set; }
        public uint pages { get; set; }
        public List<Message> messages { get; set; }

        public override string ToString()
        {
            return string.Format("Page {0} of {1}", this.currentPage, this.pages);
        }
    }
}
