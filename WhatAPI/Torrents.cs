using System;
using System.Collections.Generic;

namespace What
{
    public class Torrents
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return status;
        }

        public class Artist
        {
            public uint id { get; set; }
            public string name { get; set; }
            public uint aliasid { get; set; }

            public override string ToString()
            {
                return name;
            }
        }

        public class Response
        {
            public uint currentPage { get; set; }
            public uint pages { get; set; }
            public List<Result> results { get; set; }

            public override string ToString()
            {
                return string.Format("Page {0} of {1}", currentPage, pages);
            }
        }

        public class Result
        {
            public uint groupId { get; set; }
            public string groupName { get; set; }
            public string artist { get; set; }
            public List<string> tags { get; set; }
            public bool bookmarked { get; set; }
            public bool vanityHouse { get; set; }
            public uint groupYear { get; set; }
            public string releaseType { get; set; }
            public string groupTime { get; set; }
            public long maxSize { get; set; }
            public uint totalSnatched { get; set; }
            public uint totalSeeders { get; set; }
            public uint totalLeechers { get; set; }
            public List<Torrent> torrents { get; set; }
            public uint? torrentId { get; set; }
            public string category { get; set; }
            public uint fileCount { get; set; }
            public long size { get; set; }
            public uint snatches { get; set; }
            public uint seeders { get; set; }
            public uint leechers { get; set; }
            public bool isFreeleech { get; set; }
            public bool isNeutralLeech { get; set; }
            public bool isPersonalFreeleech { get; set; }
            public bool canUseToken { get; set; }
            public bool hasSnatched { get; set; }

            public override string ToString()
            {
                return string.Format("{0} - {1}", artist, groupName);
            }
        }

        public class Torrent
        {
            public uint torrentId { get; set; }
            public uint editionId { get; set; }
            public List<Artist> artists { get; set; }
            public bool remastered { get; set; }
            public uint remasterYear { get; set; }
            public string remasterCatalogueNumber { get; set; }
            public string remasterTitle { get; set; }
            public string media { get; set; }
            public string encoding { get; set; }
            public string format { get; set; }
            public bool hasLog { get; set; }
            public int logScore { get; set; } // Can be negative value
            public bool hasCue { get; set; }
            public bool scene { get; set; }
            public bool vanityHouse { get; set; }
            public uint fileCount { get; set; }
            public DateTime time { get; set; }
            public long size { get; set; }
            public uint snatches { get; set; }
            public uint seeders { get; set; }
            public uint leechers { get; set; }
            public bool isFreeleech { get; set; }
            public bool isNeutralLeech { get; set; }
            public bool isPersonalFreeleech { get; set; }
            public bool canUseToken { get; set; }
            public bool hasSnatched { get; set; }

            public override string ToString()
            {
                return string.Format("{0} {1}", encoding, format);
            }
        }
    }
}