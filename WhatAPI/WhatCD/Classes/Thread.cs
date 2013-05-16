using System;
using System.Collections.Generic;

namespace WhatCD
{
    public class Thread
    {
        public string status { get; set; }
        public Response response { get; set; }

        public override string ToString()
        {
            return this.status;
        }

        public class Answer
        {
            public string answer { get; set; }
            public double ratio { get; set; }
            public double percent { get; set; }

            public override string ToString()
            {
                return this.answer;
            }
        }

        public class Author
        {
            public uint authorId { get; set; }
            public string authorName { get; set; }
            public List<string> paranoia { get; set; }
            public bool artist { get; set; }
            public bool donor { get; set; }
            public bool warned { get; set; }
            public string avatar { get; set; }
            public bool enabled { get; set; }
            public string userTitle { get; set; }

            public override string ToString()
            {
                return this.authorName;
            }
        }

        public class Poll
        {
            public bool closed { get; set; }
            public string featured { get; set; }
            public string question { get; set; }
            public uint maxVotes { get; set; }
            public uint totalVotes { get; set; }
            public bool voted { get; set; }
            public List<Answer> answers { get; set; }

            public override string ToString()
            {
                return this.question;
            }
        }

        public class Post
        {
            public uint postId { get; set; }
            public DateTime addedTime { get; set; }
            public string bbBody { get; set; }
            public string body { get; set; }
            public uint editedUserId { get; set; }
            public string editedTime { get; set; }
            public string editedUsername { get; set; }
            public Author author { get; set; }

            public override string ToString()
            {
                return this.body;
            }
        }

        public class Response
        {
            public uint forumId { get; set; }
            public string forumName { get; set; }
            public uint threadId { get; set; }
            public string threadTitle { get; set; }
            public bool subscribed { get; set; }
            public bool locked { get; set; }
            public bool sticky { get; set; }
            public uint currentPage { get; set; }
            public uint pages { get; set; }
            public Poll poll { get; set; }
            public List<Post> posts { get; set; }

            public override string ToString()
            {
                return this.threadTitle;
            }
        }
    }
}