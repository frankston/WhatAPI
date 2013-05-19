using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionArtist
{
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
            return this.title;
        }
    }
}
