using System;
using System.Collections.Generic;

namespace What
{
    public class Conversation
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return status;
        }

        public class Message
        {
            public uint messageId { get; set; }
            public uint senderId { get; set; }
            public string senderName { get; set; }
            public DateTime sentDate { get; set; }
            public string bbBody { get; set; }
            public string body { get; set; }

            public override string ToString()
            {
                return body;
            }
        }

        public class Response
        {
            public uint convId { get; set; }
            public string subject { get; set; }
            public bool sticky { get; set; }
            public List<Message> messages { get; set; }

            public override string ToString()
            {
                return subject;
            }
        }
    }
}