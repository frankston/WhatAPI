using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model
{
    /// <summary>
    /// Note: If no arguments are specified then the most recent requests are shown.
    /// </summary>
    public interface ISearchRequests
    {
        /// <summary>
        /// Term to search for.
        /// Optional.
        /// </summary>
        string SearchTerm { get; set; }

        /// <summary>
        /// Page number to display (default: 1).
        /// Optional.
        /// </summary>
        int? Page { get; set; }

        /// <summary>
        /// Include filled requests in results (default: false).
        /// Optional.
        /// </summary>
        bool ShowFilled { get; set; }

        /// <summary>
        /// Tags to search by (comma separated).
        /// Optional.
        /// </summary>
        string Tags { get; set; }

        /// <summary>
        /// Acceptable values:
        /// MatchAll = 1
        /// MatchAny = 0
        /// Optional.
        /// </summary>
        int? TagType { get; set; }
    }
}
