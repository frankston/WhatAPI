﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionAnnouncements
{
    public class Announcement
    {
        public int newsId { get; set; }
        public string title { get; set; }
        public string bbBody { get; set; }
        public string body { get; set; }
        public DateTime newsTime { get; set; }

        public override string ToString()
        {
            return this.title;
        }
    }
}
