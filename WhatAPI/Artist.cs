using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace What
{
    public class Artist
    {
        public string status { get; set; }
        public Response response { get; set; }
        public override string ToString()
        {
            return status;
        }

        public class Tag
        {
            public string name { get; set; }
            public uint count { get; set; }
            public override string ToString()
            {
                return name;
            }
        }

        public class Statistics
        {
            public uint numGroups { get; set; }
            public uint numTorrents { get; set; }
            public uint numSeeders { get; set; }
            public uint numLeechers { get; set; }
            public uint numSnatches { get; set; }
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
            public override string ToString()
            {
                return remasterTitle;
            }
        }

        public class Torrentgroup
        {
            public uint groupId { get; set; }
            public string groupName { get; set; }
            public uint groupYear { get; set; }
            public string groupRecordLabel { get; set; }
            public string groupCatalogueNumber { get; set; }
            public List<string> tags { get; set; }
            public ReleaseType releaseType { get; set; }
            public bool groupVanityHouse { get; set; }
            public bool hasBookmarked { get; set; }
            public List<Torrent> torrent { get; set; }
            public override string ToString()
            {
                return groupName;
            }
        }

        public class Request
        {
            public uint requestId { get; set; }
            public uint categoryId { get; set; }
            public string title { get; set; }
            public uint year { get; set; }
            public DateTime timeAdded { get; set; }
            public uint votes { get; set; }
            public long bounty { get; set; }
            public override string ToString()
            {
                return title;
            }
        }

        public class SimilarArtist
        {
            public int artistId { get; set; }
            public string name { get; set; }
            public int score { get; set; }
            public int similarId { get; set; }
            public override string ToString()
            {
                return name;
            }
        }

        public class Response
        {
            public uint id { get; set; }
            public string name { get; set; }
            public bool notificationsEnabled { get; set; }
            public bool hasBookmarked { get; set; }
            public string image { get; set; }
            public string body { get; set; }
            public bool vanityHouse { get; set; }
            public List<Tag> tags { get; set; }
            public List<SimilarArtist> similarArtists { get; set; }
            public Statistics statistics { get; set; }
            public List<Torrentgroup> torrentgroup { get; set; }
            public List<Request> requests { get; set; }
            public override string ToString()
            {
                return name;
            }
        }

    }
}
