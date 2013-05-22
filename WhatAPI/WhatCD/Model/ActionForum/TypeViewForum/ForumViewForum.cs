using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model.ActionForum.TypeViewForum
{
    public class ForumViewForum : IResponse<Response>
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }
    }
}