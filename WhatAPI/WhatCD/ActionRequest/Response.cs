using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionRequest
{
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
        public ReleaseType releaseType { get; set; }
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
            return this.title;
        }
    }
}
