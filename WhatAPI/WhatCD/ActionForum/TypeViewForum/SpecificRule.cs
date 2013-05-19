using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionForum.TypeViewForum
{
    public class SpecificRule
    {
        public uint threadId { get; set; }
        public string thread { get; set; }

        public override string ToString()
        {
            return this.thread;
        }
    }
}
