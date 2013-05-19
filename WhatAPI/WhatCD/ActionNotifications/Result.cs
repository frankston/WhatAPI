using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionNotifications
{
    public class Result
    {
        public uint torrentId { get; set; }
        public uint groupId { get; set; }
        public string groupName { get; set; }
        public uint groupCategoryId { get; set; }
        public string torrentTags { get; set; }
        public long size { get; set; }
        public uint fileCount { get; set; }
        public string format { get; set; }
        public string encoding { get; set; }
        public string media { get; set; }
        public bool scene { get; set; }
        public uint groupYear { get; set; }
        public uint remasterYear { get; set; }
        public string remasterTitle { get; set; }
        public uint snatched { get; set; }
        public uint seeders { get; set; }
        public uint leechers { get; set; }
        public DateTime notificationTime { get; set; }
        public bool hasLog { get; set; }
        public bool hasCue { get; set; }
        public int logScore { get; set; } // Note: Can be negative value
        public bool freeTorrent { get; set; }
        public bool logInDb { get; set; }
        public bool unread { get; set; }

        public override string ToString()
        {
            return this.groupName;
        }
    }
}
