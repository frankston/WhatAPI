using System;
using System.Collections.Generic;

namespace What
{
    public class Request
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

            public override string ToString()
            {
                return name;
            }
        }

        public class Comment
        {
            public uint postId { get; set; }
            public uint authorId { get; set; }
            public string name { get; set; }
            public bool donor { get; set; }
            public bool warned { get; set; }
            public bool enabled { get; set; }
            public string @class { get; set; }
            public DateTime addedTime { get; set; }
            public string avatar { get; set; }
            public string comment { get; set; }
            public uint editedUserId { get; set; }
            public string editedUsername { get; set; }
            public string editedTime { get; set; }

            public override string ToString()
            {
                return comment;
            }
        }

        public class Composer
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return name;
            }
        }

        public class Conductor
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return name;
            }
        }

        public class Dj
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return name;
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
                return name;
            }
        }

        public class RemixedBy
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return name;
            }
        }

        public class Response
        {
            public uint requestId { get; set; }
            public uint requestorId { get; set; }
            public string requestorName { get; set; }
            public double requestTax { get; set; }
            public DateTime timeAdded { get; set; }
            public bool canEdit { get; set; }
            public bool canVote { get; set; }
            public uint minimumVote { get; set; }
            public uint voteCount { get; set; }
            public string lastVote { get; set; }
            public List<TopContributor> topContributors { get; set; }
            public long totalBounty { get; set; }
            public uint categoryId { get; set; }
            public string categoryName { get; set; }
            public string title { get; set; }
            public uint year { get; set; }
            public string image { get; set; }
            public string description { get; set; }
            public MusicInfo musicInfo { get; set; }
            public string catalogueNumber { get; set; }
            public uint releaseType { get; set; }
            public string releaseName { get; set; }
            public string bitrateList { get; set; }
            public string formatList { get; set; }
            public string mediaList { get; set; }
            public string logCue { get; set; }
            public bool isFilled { get; set; }
            public uint fillerId { get; set; }
            public string fillerName { get; set; }
            public uint torrentId { get; set; }
            public string timeFilled { get; set; }
            public List<string> tags { get; set; }
            public List<Comment> comments { get; set; }
            public uint commentPage { get; set; }
            public uint commentPages { get; set; }

            public override string ToString()
            {
                return title;
            }
        }

        public class TopContributor
        {
            public uint userId { get; set; }
            public string userName { get; set; }
            public long bounty { get; set; }

            public override string ToString()
            {
                return userName;
            }
        }

        public class With
        {
            public int id { get; set; }
            public string name { get; set; }

            public override string ToString()
            {
                return name;
            }
        }
    }
}