using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.Model
{
    public interface ISearchTorrents
    {
        string ArtistName { get; set; }
        string GroupName { get; set; } // torrent name
        string RecordLabel { get; set; }
        string CatalogueNumber { get; set; }
        int? Year { get; set; }
        string RemasterTitle { get; set; }
        string RemasterYear { get; set; }
        string RemasterRecordLabel { get; set; }
        string RemasterCatalogueNumber { get; set; }
        string FileList { get; set; }
        string Encoding { get; set; }
        string Format { get; set; }
        string Media { get; set; }
        string ReleaseType { get; set; }
        bool? HasLog { get; set; }
        bool? HasCue { get; set; }
        bool? Scene { get; set; }
        bool? VanityHouse { get; set; }
        string FreeTorrent { get; set; }
        string TagList { get; set; }
        string TagsType { get; set; }
        string OrderBy { get; set; }
        string OrderWay { get; set; }
        string GroupResults { get; set; }
        string SearchTerm { get; set; }
        int? Page { get; set; }
    }
}
