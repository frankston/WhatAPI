﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionRequests
{
    public class Result
    {
        public uint requestId { get; set; }
        public uint requestorId { get; set; }
        public string requestorName { get; set; }
        public DateTime timeAdded { get; set; }
        public DateTime lastVote { get; set; }
        public uint voteCount { get; set; }
        public long bounty { get; set; }
        public uint categoryId { get; set; }
        public string categoryName { get; set; }
        public List<List<Artist>> artists { get; set; }
        public string title { get; set; }
        public uint year { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public string catalogueNumber { get; set; }
        public string releaseType { get; set; }
        public string bitrateList { get; set; }
        public string formatList { get; set; }
        public string mediaList { get; set; }
        public string logCue { get; set; }
        public bool isFilled { get; set; }
        public uint fillerId { get; set; }
        public string fillerName { get; set; }
        public uint torrentId { get; set; }
        public string timeFilled { get; set; }

        public override string ToString()
        {
            return this.title;
        }
    }
}
