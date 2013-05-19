using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionIndex
{
    public class Notifications
    {
        public uint messages { get; set; }
        public uint notifications { get; set; }
        public bool newAnnouncement { get; set; }
        public bool newBlog { get; set; }
    }
}
