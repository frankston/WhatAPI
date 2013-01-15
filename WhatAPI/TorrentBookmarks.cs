using System;
using System.Collections.Generic;

namespace What
{
    public class TorrentBookmarks
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return status;
        }

        public class Bookmark
        {
            public uint id { get; set; }
            public string name { get; set; }
            public uint year { get; set; }
            public string recordLabel { get; set; }
            public string catalogueNumber { get; set; }
            public string tagList { get; set; }
            public string releaseType { get; set; }
            public bool vanityHouse { get; set; }
            public string image { get; set; }
            public List<Torrent> torrents { get; set; }

            public override string ToString()
            {
                return name;
            }
        }

        public class Response
        {
            public List<Bookmark> bookmarks { get; set; }
        }

        public class Torrent
        {
            public uint id { get; set; }
            public uint groupId { get; set; }
            public string media { get; set; }
            public string format { get; set; }
            public string encoding { get; set; }
            public uint remasterYear { get; set; }
            public bool remastered { get; set; }
            public string remasterTitle { get; set; }
            public string remasterRecordLabel { get; set; }
            public string remasterCatalogueNumber { get; set; }
            public bool scene { get; set; }
            public bool hasLog { get; set; }
            public bool hasCue { get; set; }
            public int logScore { get; set; } // Can be negative value
            public uint fileCount { get; set; }
            public bool freeTorrent { get; set; }
            public long size { get; set; }
            public uint leechers { get; set; }
            public uint seeders { get; set; }
            public uint snatched { get; set; }
            public DateTime time { get; set; }
            public uint hasFile { get; set; }
        }
    }
}