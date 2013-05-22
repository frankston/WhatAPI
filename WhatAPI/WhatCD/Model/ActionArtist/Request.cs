using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionArtist
{
    public class Request
    {
        public int requestId { get; set; }
        public int categoryId { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public DateTime timeAdded { get; set; }
        public int votes { get; set; }
        public long bounty { get; set; }
        public override string ToString()
        {
            return this.title;
        }
    }
}
