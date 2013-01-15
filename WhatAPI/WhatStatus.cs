namespace What
{
    public class WhatStatus
    {
        public Status status { get; set; }
        public Uptime uptime { get; set; }
        public Records records { get; set; }

        public class Records
        {
            public string site { get; set; }
            public string tracker { get; set; }
            public string irc { get; set; }

            public override string ToString()
            {
                return site;
            }
        }

        public class Status
        {
            public string site { get; set; }
            public string tracker { get; set; }
            public string irc { get; set; }

            public override string ToString()
            {
                return site;
            }
        }

        public class Uptime
        {
            public string site { get; set; }
            public string tracker { get; set; }
            public string irc { get; set; }

            public override string ToString()
            {
                return site;
            }
        }
    }
}