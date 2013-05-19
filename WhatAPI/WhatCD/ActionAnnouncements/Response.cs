using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionAnnouncements
{
    public class Response
    {
        public List<Announcement> announcements { get; set; }
        public List<BlogPost> blogPosts { get; set; }
    }
}
