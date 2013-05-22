using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionIndex
{
    public class Notifications
    {
        public int messages { get; set; }
        public int notifications { get; set; }
        public bool newAnnouncement { get; set; }
        public bool newBlog { get; set; }
        public bool newSubscriptions { get; set; }
    }
}
