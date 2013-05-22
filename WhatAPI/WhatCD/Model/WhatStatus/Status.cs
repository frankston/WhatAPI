using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace WhatCD.Model.WhatStatus
{
    public class Status
    {
        [JsonConverter(typeof(BoolConverter))]
        public bool site { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool irc { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        public bool tracker { get; set; }
    }
}
