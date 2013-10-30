using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatCD.Model.ActionCollage
{
    public class Response
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int creatorID { get; set; }
        public bool deleted { get; set; }
        public int collageCategoryID { get; set; }
        public string collageCategoryName { get; set; }
        public bool locked { get; set; }
        public int maxGroups { get; set; }
        public int maxGroupsPerUser { get; set; }
        public bool hasBookmarked { get; set; }
        public int subscriberCount { get; set; }
        public List<string> torrentGroupIDList { get; set; }
        public List<Torrentgroup> torrentgroups { get; set; }

        public override string ToString()
        {
            return name.ToString();
        }
    }
}