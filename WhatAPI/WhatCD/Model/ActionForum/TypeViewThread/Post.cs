using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionForum.TypeViewThread
{
    public class Post
    {
        public int postId { get; set; }
        public DateTime addedTime { get; set; }
        public string bbBody { get; set; }
        public string body { get; set; }
        public int editedUserId { get; set; }
        public string editedTime { get; set; }
        public string editedUsername { get; set; }
        public Author author { get; set; }

        public override string ToString()
        {
            return this.body;
        }
    }
}
