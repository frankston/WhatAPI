using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatCD.ActionSimilarArtists
{
    // TODO: raise bug that this class does not confirm to expected json structure - cannot implment IResponse interface
    public class SimilarArtists
    {
        public List<Artist> artists { get; set; }
    }
}