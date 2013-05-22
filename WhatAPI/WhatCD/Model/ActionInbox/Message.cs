using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionInbox
{
    public class Message
    {
        public int convId { get; set; }
        public string subject { get; set; }
        public bool unread { get; set; }
        public bool sticky { get; set; }
        public int forwardedId { get; set; }
        public string forwardedName { get; set; }
        public int senderId { get; set; }
        public string username { get; set; }
        public bool donor { get; set; }
        public bool warned { get; set; }
        public bool enabled { get; set; }
        public DateTime date { get; set; }

        public override string ToString()
        {
            return this.subject;
        }
    }
}
