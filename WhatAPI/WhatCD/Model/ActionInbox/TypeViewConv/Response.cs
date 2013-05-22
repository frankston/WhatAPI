using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionInbox.TypeViewConv
{
    public class Response
    {
        public int convId { get; set; }
        public string subject { get; set; }
        public bool sticky { get; set; }
        public List<Message> messages { get; set; }

        public override string ToString()
        {
            return this.subject;
        }
    }
}
