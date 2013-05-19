using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionRequest
{
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
            return this.comment;
        }
    }
}
