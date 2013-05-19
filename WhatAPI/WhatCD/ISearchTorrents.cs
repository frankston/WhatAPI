using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD
{
    public interface ISearchTorrents
    {
        string ArtistName { get; set; }
        string GroupName { get; set; } // torrent name
        string RecordLabel { get; set; }
        string CatalogueNumber { get; set; }
        string Year { get; set; }
        string RemasterTitle { get; set; }
        string RemasterYear { get; set; }
        string RemasterRecordLabel { get; set; }
        string RemasterCatalogueNumber { get; set; }
        string FileList { get; set; }
        string Encoding { get; set; }
        string Format { get; set; } // eg. "FLAC"
        string Media { get; set; }
        string ReleaseType { get; set; }
        string HasLog { get; set; }
        string HasCue { get; set; }
        string Scene { get; set; }
        string VanityHouse { get; set; }
        string FreeTorrent { get; set; }
        string TagList { get; set; }
        string TagsType { get; set; }
        string OrderBy { get; set; }
        string OrderWay { get; set; }
        string GroupResults { get; set; }
        string SearchTerm { get; set; }
        string Page { get; set; }
    }
}
