using System.Collections.Generic;

namespace What
{
    public class Top10Tags
    {
        public string status { get; set; }
        public List<Response> response { get; set; }

        public override string ToString()
        {
            return status;
        }

        public class Response
        {
            public string caption { get; set; }
            public string tag { get; set; }
            public uint limit { get; set; }
            public List<Result> results { get; set; }

            public override string ToString()
            {
                return caption;
            }
        }

        public class Result
        {
            public string name { get; set; }
            public uint uses { get; set; }
            public uint posVotes { get; set; }
            public uint negVotes { get; set; }

            public override string ToString()
            {
                return name;
            }
        }
    }
}