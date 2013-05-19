using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD
{
    public interface IInbox
    {
        /// <summary>
        /// Type of messages to search.
        /// Acceptable values: "inbox", "sentbox"
        /// Default value is "inbox".
        /// Optional.
        /// </summary>
        string MessageType { get; set; }

        /// <summary>
        /// Display unread messages first?
        /// Default value is false.
        /// Optional.
        /// </summary>
        bool DisplayUnreadFirst { get; set; }

        /// <summary>
        /// Page to display.
        /// Default value is 1.
        /// Optional.
        /// </summary>
        uint? Page { get; set; }

        /// <summary>
        /// Area to search.
        /// Acceptable values: "subject", "message", "user".
        /// Optional.
        /// </summary>
        string SearchType { get; set; }

        /// <summary>
        /// Filter messages by search term.
        /// Optional.
        /// </summary>
        string SearchTerm { get; set; }
    }
}
