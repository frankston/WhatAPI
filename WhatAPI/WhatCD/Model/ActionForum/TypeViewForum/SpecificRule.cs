using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionForum.TypeViewForum
{
    public class SpecificRule
    {
        public int threadId { get; set; }
        public string thread { get; set; }

        public override string ToString()
        {
            return this.thread;
        }
    }
}
