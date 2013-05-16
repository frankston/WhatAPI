using System.Collections.Generic;

namespace WhatCD
{
    public class ArtistBookmarks
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Artist
        {
            public uint artistId { get; set; }
            public string artistName { get; set; }

            public override string ToString()
            {
                return this.artistName;
            }
        }

        public class Response
        {
            public List<Artist> artists { get; set; }
        }
    }
}