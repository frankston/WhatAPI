using System.Collections.Generic;

namespace What
{
    public class SimilarArtists
    {
        public List<Artist> artists { get; set; }

        public class Artist
        {
            public uint id { get; set; }
            public string name { get; set; }
            public uint score { get; set; }

            public override string ToString()
            {
                return name;
            }
        }
    }
}