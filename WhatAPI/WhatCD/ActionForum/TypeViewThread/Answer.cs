using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeViewThread
{
    public class Answer
    {
        public string answer { get; set; }
        public double ratio { get; set; }
        public double percent { get; set; }

        public override string ToString()
        {
            return this.answer;
        }
    }
}
