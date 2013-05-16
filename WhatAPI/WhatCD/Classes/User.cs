using System;

namespace WhatCD
{
    public class User
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Community
        {
            public uint? posts { get; set; }
            public uint? torrentComments { get; set; }
            public uint? collagesStarted { get; set; }
            public uint? collagesContrib { get; set; }
            public uint? requestsFilled { get; set; }
            public uint? requestsVoted { get; set; }
            public uint? perfectFlacs { get; set; }
            public long? uploaded { get; set; }
            public uint? groups { get; set; }
            public uint? seeding { get; set; }
            public uint? leeching { get; set; }
            public uint? snatched { get; set; }
            public uint? invited { get; set; }
        }

        public class Personal
        {
            public string @class { get; set; }
            public uint paranoia { get; set; }
            public string paranoiaText { get; set; }
            public bool donor { get; set; }
            public bool warned { get; set; }
            public bool enabled { get; set; }
            public string passkey { get; set; }

            public override string ToString()
            {
                return this.@class;
            }
        }

        public class Ranks
        {
            public long? uploaded { get; set; }
            public long? downloaded { get; set; }
            public long? uploads { get; set; }
            public uint? requests { get; set; }
            public long? bounty { get; set; }
            public uint? posts { get; set; }
            public uint? artists { get; set; }
            public uint? overall { get; set; }
        }

        public class Response
        {
            public string username { get; set; }
            public string avatar { get; set; }
            public bool isFriend { get; set; }
            public string profileText { get; set; }
            public Stats stats { get; set; }
            public Ranks ranks { get; set; }
            public Personal personal { get; set; }
            public Community community { get; set; }

            public override string ToString()
            {
                return this.username;
            }
        }

        public class Stats
        {
            public DateTime joinedDate { get; set; }
            public DateTime? lastAccess { get; set; }
            public long? uploaded { get; set; }
            public long? downloaded { get; set; }
            public double? ratio { get; set; }
            public double? requiredRatio { get; set; }
        }
    }
}