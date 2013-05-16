using System;
using System.Collections.Generic;

namespace WhatCD
{
    public class TorrentGroup
    {
        // TODO: Known HTML return issue: https://what.cd/forums.php?action=viewthread&threadid=169772

        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Artist
        {
            public uint id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class Composer
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class Conductor
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class Dj
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class Group
        {
            public string wikiBody { get; set; }
            public string wikiImage { get; set; }
            public uint id { get; set; }
            public string name { get; set; }
            public uint year { get; set; }
            public string recordLabel { get; set; }
            public string catalogueNumber { get; set; }
            public ReleaseType releaseType { get; set; }
            public uint categoryId { get; set; }
            public string categoryName { get; set; }
            public DateTime time { get; set; }
            public bool vanityHouse { get; set; }
            public MusicInfo musicInfo { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class MusicInfo
        {
            public List<Composer> composers { get; set; }
            public List<Dj> dj { get; set; }
            public List<Artist> artists { get; set; }
            public List<With> with { get; set; }
            public List<Conductor> conductor { get; set; }
            public List<RemixedBy> remixedBy { get; set; }
            public List<Producer> producer { get; set; }
        }

        public class Producer
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class RemixedBy
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }

        public class Response
        {
            public Group group { get; set; }
            public List<Torrent> torrents { get; set; }
        }

        public class Torrent
        {
            public uint id { get; set; }
            public string media { get; set; }
            public string format { get; set; }
            public string encoding { get; set; }
            public bool remastered { get; set; }
            public uint remasterYear { get; set; }
            public string remasterTitle { get; set; }
            public string remasterRecordLabel { get; set; }
            public string remasterCatalogueNumber { get; set; }
            public bool scene { get; set; }
            public bool hasLog { get; set; }
            public bool hasCue { get; set; }
            public int logScore { get; set; } // Note: Can be negative value
            public uint fileCount { get; set; }
            public uint size { get; set; }
            public uint seeders { get; set; }
            public uint leechers { get; set; }
            public uint snatched { get; set; }
            public bool freeTorrent { get; set; }
            public DateTime time { get; set; }
            public string description { get; set; }
            public string fileList { get; set; }
            public string filePath { get; set; }
            public uint userId { get; set; }
            public string username { get; set; }

            public override string ToString()
            {
                return this.filePath;
            }
        }

        public class With
        {
            public uint id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return this.name;
            }
        }
    }
}