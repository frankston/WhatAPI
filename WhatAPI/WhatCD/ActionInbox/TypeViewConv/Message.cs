﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionInbox.TypeViewConv
{
    public class Message
    {
        public uint messageId { get; set; }
        public uint senderId { get; set; }
        public string senderName { get; set; }
        public DateTime sentDate { get; set; }
        public string bbBody { get; set; }
        public string body { get; set; }

        public override string ToString()
        {
            return this.body;
        }
    }
}
