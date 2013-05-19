using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionUserSearch
{
    public class UserSearch : IResponse<Response>
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }
    }
}