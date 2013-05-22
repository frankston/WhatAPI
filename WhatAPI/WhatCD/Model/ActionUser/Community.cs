using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionUser
{
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
}
