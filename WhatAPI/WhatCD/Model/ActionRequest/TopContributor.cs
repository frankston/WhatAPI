using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionRequest
{
    public class TopContributor
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public long bounty { get; set; }

        public override string ToString()
        {
            return this.userName;
        }
    }
}
