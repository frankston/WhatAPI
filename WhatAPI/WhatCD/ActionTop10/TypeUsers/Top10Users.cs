using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionTop10.TypeUsers
{
    public class Top10Users : IResponse<List<Response>>
    {
        public string status { get; set; }
        public List<Response> response { get; set; }

        public override string ToString()
        {
            return this.status;
        }
    }
}