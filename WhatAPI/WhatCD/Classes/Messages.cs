using System;
using System.Collections.Generic;

namespace WhatCD
{
    public class Messages
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Message
        {
            public uint convId { get; set; }
            public string subject { get; set; }
            public bool unread { get; set; }
            public bool sticky { get; set; }
            public uint forwardedId { get; set; }
            public string forwardedName { get; set; }
            public uint senderId { get; set; }
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

        public class Response
        {
            public uint currentPage { get; set; }
            public uint pages { get; set; }
            public List<Message> messages { get; set; }

            public override string ToString()
            {
                return string.Format("Page {0} of {1}", this.currentPage, this.pages);
            }
        }
    }
}