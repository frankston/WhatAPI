using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD
{
    public interface IGetRequest
    {
        /// <summary>
        /// Request ID.
        /// Mandatory.
        /// </summary>
        uint RequestID { get; set; }

        /// <summary>
        /// Comment page number.
        /// If null then default is last page.
        /// Optional.
        /// </summary>
        uint? CommentPage { get; set; }
    }
}
