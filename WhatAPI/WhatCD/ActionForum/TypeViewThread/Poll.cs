using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeViewThread
{
    public class Poll
    {
        public bool closed { get; set; }
        public string featured { get; set; }
        public string question { get; set; }
        public uint maxVotes { get; set; }
        public uint totalVotes { get; set; }
        public bool voted { get; set; }
        public List<Answer> answers { get; set; }

        public override string ToString()
        {
            return this.question;
        }
    }
}
