using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionNotifications
{
    public class Result
    {
        public int torrentId { get; set; }
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int groupCategoryId { get; set; }
        public string wikiImage { get; set; }
        public string torrentTags { get; set; }
        public long size { get; set; }
        public int fileCount { get; set; }
        public string format { get; set; }
        public string encoding { get; set; }
        public string media { get; set; }
        public bool scene { get; set; }
        public int groupYear { get; set; }
        public int remasterYear { get; set; }
        public string remasterTitle { get; set; }
        public int snatched { get; set; }
        public int seeders { get; set; }
        public int leechers { get; set; }
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
