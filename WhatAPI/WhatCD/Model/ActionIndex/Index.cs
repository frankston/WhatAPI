using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionIndex
{
    public class Index : IResponse<Response>
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }
    }
}