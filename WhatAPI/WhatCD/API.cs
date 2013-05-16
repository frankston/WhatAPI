using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace WhatCD
{
    public class API : IDisposable
    {

        private Uri rootWhatStatusURI = new Uri("http://whatstatus.info");
        public Uri RootWhatStatusURI
        {
            get { return this.rootWhatStatusURI; }
            set { this.rootWhatStatusURI = value; }
        }

        private Uri rootWhatCDURI = new Uri("https://what.cd");
        public Uri RootWhatCDURI
        {
            get { return this.rootWhatCDURI; }
            set { this.rootWhatCDURI = value; }
        }

        private CookieContainer cookieJar = new CookieContainer();
        public CookieContainer CookieJar
        {
            get { return this.cookieJar; }
            private set { this.cookieJar = value; }
        }


        public API(string username, string password)
        {
            this.Login(Uri.EscapeDataString(username), Uri.EscapeDataString(password));
        }

        // Forums

        /// <summary>
        /// Get forum scriptions.
        /// </summary>
        /// <param name="onlyShowUnread">Only show subscribed forums with unread threads.</param>
        /// <returns>Subscriptions object.</returns>
        public Subscriptions GetSubscriptions(bool onlyShowUnread)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=subscriptions&showunread={0}", onlyShowUnread ? 1 : 0));
            return JsonConvert.DeserializeObject<Subscriptions>(Json);
        }

        /// <summary>
        /// Gets all forum categories. 
        /// </summary>
        /// <returns>ForumCategories object.</returns>
        public ForumCategories GetForumCategories()
        {
            string Json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=forum&type=main");
            return JsonConvert.DeserializeObject<ForumCategories>(Json);
        }

        /// <summary>
        /// Gets a page of threads from a forum.
        /// </summary>
        /// <param name="forumID">Forum ID.</param>
        /// <param name="page">Page number.</param>
        /// <returns>Forum object.</returns>
        public Forum GetForum(uint forumID, uint page)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=forum&type=viewforum&forumid={0}&page={1}", forumID, page));
            return JsonConvert.DeserializeObject<Forum>(Json);
        }

        /// <summary>
        /// Gets a specific page of forum thread posts.
        /// </summary>
        /// <param name="threadID">Thread ID.</param>
        /// <param name="page">Page number.</param>
        /// <returns>Thread object.</returns>
        public Thread GetForumPageByThread(uint threadID, uint page)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=forum&type=viewthread&threadid={0}&page={1}", threadID, page));
            return JsonConvert.DeserializeObject<Thread>(Json);
        }

        /// <summary>
        /// Gets a page of forum thread posts where a specific thread exists.
        /// </summary>
        /// <param name="postID">Post ID.</param>
        /// <returns>Thread object.</returns>
        public Thread GetForumThreadByPost(uint postID)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=forum&type=viewthread&postid={0}", postID));
            return JsonConvert.DeserializeObject<Thread>(Json);
        }

        // Messages

        /// <summary>
        /// Searches the contents of messages.
        /// </summary>
        /// <param name="options">Object inheriting ISearchMessages interface.</param>
        /// <returns>Messages object.</returns>
        public Messages SearchMessages(ISearchMessages options)
        {
            // Construct query string
            StringBuilder query = new StringBuilder();
            query.Append("ajax.php?action=inbox");
            if (options.DisplayUnreadFirst) query.Append("&sort=unread");
            if (!string.IsNullOrWhiteSpace(options.MessageType)) query.Append(string.Format("&type={0}", Uri.EscapeDataString(options.MessageType)));
            if (options.Page != null && options.Page > 0) query.Append(string.Format("&page={0}", options.Page));
            if (!string.IsNullOrWhiteSpace(options.SearchTerm)) query.Append(string.Format("&search={0}", Uri.EscapeDataString(options.SearchTerm)));
            if (!string.IsNullOrWhiteSpace(options.SearchType)) query.Append(string.Format("&searchtype={0}", Uri.EscapeDataString(options.SearchType)));

            string Json = this.RequestJson(this.RootWhatCDURI, query.ToString());
            return JsonConvert.DeserializeObject<Messages>(Json);
        }

        /// <summary>
        /// Gets all messages from a conversation.
        /// </summary>
        /// <param name="conversationID">Conversation ID.</param>
        /// <returns>Conversation object.</returns>
        public Conversation GetConversation(uint conversationID)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=inbox&type=viewconv&id={0}", conversationID));
            return JsonConvert.DeserializeObject<Conversation>(Json);
        }

        // Misc

        /// <summary>
        /// Gets information about the current user.
        /// </summary>
        /// <returns>Index object.</returns>
        public Index GetIndex()
        {
            string Json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=index");
            return JsonConvert.DeserializeObject<Index>(Json);
        }

        /// <summary>
        /// Gets what.cd site, irc, and tracker status.
        /// </summary>
        /// <returns>WhatStatus object.</returns>
        public WhatStatus GetStatus()
        {
            string Json = this.RequestJson(this.RootWhatStatusURI, "json.php");
            return JsonConvert.DeserializeObject<WhatStatus>(Json);
        }

        /// <summary>
        /// Gets a quote from Rippy.
        /// </summary>
        /// <returns>Rippy quote.</returns>
        public string GetRippy()
        {
            return this.RequestJson(this.RootWhatCDURI, "ajax.php?action=rippy");
        }

        /// <summary>
        /// Gets the most recent announcements and blog posts.
        /// </summary>
        /// <returns>Announcements object.</returns>
        public Announcements GetAnnouncements()
        {
            string Json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=announcements");
            return JsonConvert.DeserializeObject<Announcements>(Json);
        }

        /// <summary>
        /// Gets notifications.
        /// Issue raised: "currentPages" property should be "currentPage": https://what.cd/forums.php?action=viewthread&threadid=169781
        /// </summary>
        /// <param name="page">Notification page number.</param>
        /// <returns>Notifications object.</returns>
        public Notifications GetNotifications(uint page)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=notifications&page={0}", page));
            return JsonConvert.DeserializeObject<Notifications>(Json);
        }

        /// <summary>
        /// Gets the top torrents.
        /// Null Top10 torrents issue raised: https://what.cd/forums.php?action=viewthread&threadid=169763
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, and 100</param>
        /// <returns>Top10Torrents object.</returns>
        public Top10Torrents GetTop10Torrents(uint limit)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=top10&type=torrents&limit={0}", limit));
            return JsonConvert.DeserializeObject<Top10Torrents>(Json);
        }

        /// <summary>
        /// Gets the top tags.
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, and 100</param>
        /// <returns>Top10Tags object.</returns>
        public Top10Tags GetTop10Tags(uint limit)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=top10&type=tags&limit={0}", limit));
            return JsonConvert.DeserializeObject<Top10Tags>(Json);
        }

        /// <summary>
        /// Gets the top users.
        /// Known null return issue raised: https://what.cd/forums.php?action=viewthread&threadid=169763
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, and 100</param>
        /// <returns>Top10Users object.</returns>
        public Top10Users GetTop10Users(uint limit)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=top10&type=users&limit={0}", limit));
            return JsonConvert.DeserializeObject<Top10Users>(Json);
        }

        /// <summary>
        /// Logs off the current session.
        /// </summary>
        public void Dispose()
        {
            this.Logoff();
        }

        // Requests

        /// <summary>
        /// Known issue - MusicInfo object sometimes returning as an empty array: https://what.cd/forums.php?action=viewthread&threadid=169790
        /// </summary>
        /// <param name="options">Object that inherits IGetRequest</param>
        /// <returns>Request object</returns>
        public Request GetRequest(IGetRequest options)
        {
            StringBuilder Query = new StringBuilder();
            Query.Append("ajax.php?action=request");
            Query.Append(string.Format("&id={0}", options.RequestID));
            if (options.CommentPage != null && options.CommentPage > 0) Query.Append(string.Format("&page={0}", options.CommentPage));

            string Json = this.RequestJson(this.RootWhatCDURI, Query.ToString());
            return JsonConvert.DeserializeObject<Request>(Json);
        }

        /// <summary>
        /// Searches requests.
        /// If no arguments are specified then the most recent requests are shown.
        /// Known issue - artist array nested in an unnecessary array: https://what.cd/forums.php?action=viewthread&threadid=169787
        /// </summary>
        /// <param name="options">Object that inherits ISearchRequests.</param>
        /// <returns>Requests object.</returns>
        public Requests SearchRequests(ISearchRequests options)
        {
            StringBuilder Query = new StringBuilder();
            Query.Append(string.Format("ajax.php?action=requests&tags_type={0}&show_filled={1}", options.TagType, options.ShowFilled.ToString().ToLower()));
            if (options.Page > 0) Query.Append(string.Format("&page={0}", options.Page));
            if (!string.IsNullOrWhiteSpace(options.Tags)) Query.Append(string.Format("&tag={0}", Uri.EscapeDataString(options.Tags)));
            if (!string.IsNullOrWhiteSpace(options.SearchTerm)) Query.Append(string.Format("&search={0}", Uri.EscapeDataString(options.SearchTerm)));
            if (!string.IsNullOrWhiteSpace(options.Tags)) Query.Append(string.Format("&tags={0}", Uri.EscapeDataString(options.Tags)));

            string Json = this.RequestJson(this.RootWhatCDURI, Query.ToString());
            return JsonConvert.DeserializeObject<Requests>(Json);
        }

        // Torrents

        /// <summary>
        /// Gets similar artists.
        /// Note that the return response from the server is not properly formatted JSON
        /// </summary>
        /// <param name="artistID">Artist ID.</param>
        /// <param name="limit">Maximum result limit.</param>
        /// <returns>SimilarArtists object.</returns>
        public SimilarArtists GetSimilarArtists(uint artistID, uint limit)
        {
            string rtn = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=similar_artists&id={0}&limit={1}", artistID, limit));
            if (string.IsNullOrWhiteSpace(rtn) || string.Equals(rtn, "null", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }
            else
            {
                string Json = string.Format("{{\"artists\":{0}}}", rtn);
                return JsonConvert.DeserializeObject<SimilarArtists>(Json);
            }
        }

        /// <summary>
        /// Searches torrents.
        /// </summary>
        /// <param name="options">Object that inherits from ISearchTorrents.</param>
        /// <returns>Torrents object.</returns>
        public Torrents SearchTorrents(ISearchTorrents options)
        {
            StringBuilder Query = new StringBuilder();
            Query.Append("ajax.php?action=browse&action=advanced");

            if (!string.IsNullOrWhiteSpace(options.ArtistName)) Query.Append(string.Format("&artistname={0}", Uri.EscapeDataString(options.ArtistName)));
            if (!string.IsNullOrWhiteSpace(options.CatalogueNumber)) Query.Append(string.Format("&cataloguenumber={0}", Uri.EscapeDataString(options.CatalogueNumber)));
            if (!string.IsNullOrWhiteSpace(options.Encoding)) Query.Append(string.Format("&encoding={0}", Uri.EscapeDataString(options.Encoding)));
            if (!string.IsNullOrWhiteSpace(options.FileList)) Query.Append(string.Format("&filelist={0}", Uri.EscapeDataString(options.FileList)));
            if (!string.IsNullOrWhiteSpace(options.Format)) Query.Append(string.Format("&format={0}", Uri.EscapeDataString(options.Format)));
            if (!string.IsNullOrWhiteSpace(options.FreeTorrent)) Query.Append(string.Format("&freetorrent={0}", Uri.EscapeDataString(options.FreeTorrent)));
            if (!string.IsNullOrWhiteSpace(options.Group_Results)) Query.Append(string.Format("&group_results={0}", Uri.EscapeDataString(options.Group_Results)));
            if (!string.IsNullOrWhiteSpace(options.GroupName)) Query.Append(string.Format("&groupname={0}", Uri.EscapeDataString(options.GroupName)));
            if (!string.IsNullOrWhiteSpace(options.HasCue)) Query.Append(string.Format("&hascue={0}", Uri.EscapeDataString(options.HasCue)));
            if (!string.IsNullOrWhiteSpace(options.HasLog)) Query.Append(string.Format("&haslog={0}", Uri.EscapeDataString(options.HasLog)));
            if (!string.IsNullOrWhiteSpace(options.Media)) Query.Append(string.Format("&media={0}", Uri.EscapeDataString(options.Media)));
            if (!string.IsNullOrWhiteSpace(options.Order_By)) Query.Append(string.Format("&order_by={0}", Uri.EscapeDataString(options.Order_By)));
            if (!string.IsNullOrWhiteSpace(options.Order_Way)) Query.Append(string.Format("&order_way={0}", Uri.EscapeDataString(options.Order_Way)));
            if (!string.IsNullOrWhiteSpace(options.Page)) Query.Append(string.Format("&page={0}", Uri.EscapeDataString(options.Page)));
            if (!string.IsNullOrWhiteSpace(options.RecordLabel)) Query.Append(string.Format("&recordlabel={0}", Uri.EscapeDataString(options.RecordLabel)));
            if (!string.IsNullOrWhiteSpace(options.ReleaseType)) Query.Append(string.Format("&releasetype={0}", Uri.EscapeDataString(options.ReleaseType)));
            if (!string.IsNullOrWhiteSpace(options.RemasterCatalogueNumber)) Query.Append(string.Format("&remastercataloguenumber={0}", Uri.EscapeDataString(options.RemasterCatalogueNumber)));
            if (!string.IsNullOrWhiteSpace(options.RemasterRecordLabel)) Query.Append(string.Format("&remasterrecordlabel={0}", Uri.EscapeDataString(options.RemasterRecordLabel)));
            if (!string.IsNullOrWhiteSpace(options.RemasterTitle)) Query.Append(string.Format("&remastertitle={0}", Uri.EscapeDataString(options.RemasterTitle)));
            if (!string.IsNullOrWhiteSpace(options.RemasterYear)) Query.Append(string.Format("&remasteryear={0}", Uri.EscapeDataString(options.RemasterYear)));
            if (!string.IsNullOrWhiteSpace(options.Scene)) Query.Append(string.Format("&scene={0}", Uri.EscapeDataString(options.Scene)));
            if (!string.IsNullOrWhiteSpace(options.SearchTerm)) Query.Append(string.Format("&searchterm={0}", Uri.EscapeDataString(options.SearchTerm)));
            if (!string.IsNullOrWhiteSpace(options.TagList)) Query.Append(string.Format("&taglist={0}", Uri.EscapeDataString(options.TagList)));
            if (!string.IsNullOrWhiteSpace(options.Tags_Type)) Query.Append(string.Format("&tags_type={0}", Uri.EscapeDataString(options.Tags_Type)));
            if (!string.IsNullOrWhiteSpace(options.VanityHouse)) Query.Append(string.Format("&vanityhouse={0}", Uri.EscapeDataString(options.VanityHouse)));
            if (!string.IsNullOrWhiteSpace(options.Year)) Query.Append(string.Format("&year={0}", Uri.EscapeDataString(options.Year)));

            string Json = this.RequestJson(this.RootWhatCDURI, string.Format(Query.ToString()));
            return JsonConvert.DeserializeObject<Torrents>(Json);
        }

        /// <summary>
        /// Gets torrent bookmarks.
        /// </summary>
        /// <returns>TorrentBookmarks object.</returns>
        public TorrentBookmarks GetTorrentBookmarks()
        {
            string Json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=bookmarks&type=torrents");
            return JsonConvert.DeserializeObject<TorrentBookmarks>(Json);
        }

        /// <summary>
        /// Gets artist bookmarks.
        /// </summary>
        /// <returns>ArtistBookmarks object.</returns>
        public ArtistBookmarks GetArtistBookmarks()
        {
            string Json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=bookmarks&type=artists");
            return JsonConvert.DeserializeObject<ArtistBookmarks>(Json);
        }

        /// <summary>
        /// Gets artist information.
        /// Known Issue: Only populates similar artists the first time an artist is queried and never again: https://what.cd/forums.php?action=viewthread&threadid=169786 
        /// </summary>
        /// <param name="artistID">Artist ID.</param>
        /// <returns>Artist object.</returns>
        public Artist GetArtist(uint artistID)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=artist&id={0}", artistID));
            return JsonConvert.DeserializeObject<Artist>(Json);
        }

        /// <summary>
        /// Gets artist information.
        /// Known Issue: Only populates similar artists the first time an artist is queried and never again: https://what.cd/forums.php?action=viewthread&threadid=169786 
        /// </summary>
        /// <param name="artistName">Artist Name.</param>
        /// <returns>Artist object.</returns>
        public Artist GetArtist(string artistName)
        {
            string Json = RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=artist&artistname={0}", Uri.EscapeDataString(artistName)));
            return JsonConvert.DeserializeObject<Artist>(Json);
        }

        /// <summary>
        /// Gets a torrent group.
        /// Known HTML response issue raised: https://what.cd/forums.php?action=viewthread&threadid=169772
        /// </summary>
        /// <param name="groupID">Group ID.</param>
        /// <returns>TorrentGroup object.</returns>
        public TorrentGroup GetTorrentGroup(uint groupID)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=torrentgroup&id={0}", groupID));
            return JsonConvert.DeserializeObject<TorrentGroup>(Json);
        }

        /// <summary>
        /// Downloads the torrent file.
        /// </summary>
        /// <param name="torrentID">The ID of the torrent to download</param>
        /// <returns>A byte array with the torrent file contents.</returns>
        public byte[] DownloadTorrent(uint torrentID)
        {
            return RequestBytes(this.RootWhatCDURI, string.Format("torrents.php?action=download&id={0}", torrentID));
        }

        // Users

        /// <summary>
        /// Gets user information.
        /// Known null response issue raised: https://what.cd/forums.php?action=viewthread&threadid=169775
        /// Known invalid datetime issue: https://what.cd/forums.php?action=viewthread&threadid=169791
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>User object.</returns>
        public User GetUser(uint userID)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=user&id={0}", userID));
            return JsonConvert.DeserializeObject<User>(Json);
        }

        /// <summary>
        /// Searches for users.
        /// </summary>
        /// <param name="searchTerm">String to search for.</param>
        /// <param name="page">Results page number.</param>
        /// <returns>Users object.</returns>
        public Users SearchUsers(string searchTerm, uint page)
        {
            string Json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=usersearch&search={0}&page={1}", Uri.EscapeDataString(searchTerm), page));
            return JsonConvert.DeserializeObject<Users>(Json);
        }

        /// <summary>
        /// Determines the rip log score out of 100.
        /// </summary>
        /// <param name="log">Log file contents.</param>
        /// <returns>Score out of 100.</returns>
        public int GetFlacLogScore(string log)
        {
            HttpWebResponse Response = null;
            try
            {
                log = HttpUtility.UrlEncode(log);
                HttpWebRequest Request = WebRequest.Create(new Uri(this.RootWhatCDURI, "logchecker.php")) as HttpWebRequest;
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";
                Request.CookieContainer = this.CookieJar;

                using (StreamWriter Writer = new StreamWriter(Request.GetRequestStream()))
                {
                    Writer.Write("action=takeupload&auth={0}&log_contents={1}", this.GetIndex().response.authkey, log);
                }
                Response = Request.GetResponse() as HttpWebResponse;
                using (StreamReader Reader = new StreamReader(Response.GetResponseStream()))
                {
                    if (Response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned. Status code {0}: {1}", Response.StatusCode, Response.StatusDescription));

                    Regex RegEx = new Regex(@">(?<score>[-\d]+)</span> \(out of 100\)</blockquote>");
                    string htmlResponse = Reader.ReadToEnd();
                    MatchCollection Matches = RegEx.Matches(htmlResponse);
                    if (Matches.Count != 1) throw new WebException("Score not found");
                    int score;
                    if (!int.TryParse(Matches[0].Groups["score"].Value.ToString(), out score)) throw new Exception("Failed to convert score to int.");
                    return score;
                }
            }
            finally
            {
                Response.Close();
            }
        }

        // Interfaces

        public interface ISearchMessages
        {
            /// <summary>
            /// Type of messages to search.
            /// Acceptable values: "inbox", "sentbox"
            /// Default value is "inbox".
            /// Optional.
            /// </summary>
            string MessageType { get; set; }

            /// <summary>
            /// Display unread messages first?
            /// Default value is false.
            /// Optional.
            /// </summary>
            bool DisplayUnreadFirst { get; set; }

            /// <summary>
            /// Page to display.
            /// Default value is 1.
            /// Optional.
            /// </summary>
            uint? Page { get; set; }

            /// <summary>
            /// Area to search.
            /// Acceptable values: "subject", "message", "user".
            /// Optional.
            /// </summary>
            string SearchType { get; set; }

            /// <summary>
            /// Filter messages by search term.
            /// Optional.
            /// </summary>
            string SearchTerm { get; set; }
        }

        public interface IGetRequest
        {
            /// <summary>
            /// Request ID.
            /// Mandatory.
            /// </summary>
            uint RequestID { get; set; }

            /// <summary>
            /// Comment page number.
            /// If null then default is last page.
            /// Optional.
            /// </summary>
            uint? CommentPage { get; set; }
        }

        /// <summary>
        /// Note: If no arguments are specified then the most recent requests are shown.
        /// </summary>
        public interface ISearchRequests
        {
            /// <summary>
            /// Term to search for.
            /// Optional.
            /// </summary>
            string SearchTerm { get; set; }

            /// <summary>
            /// Page number to display (default: 1).
            /// Optional.
            /// </summary>
            int Page { get; set; }

            /// <summary>
            /// Include filled requests in results (default: false).
            /// Optional.
            /// </summary>
            bool ShowFilled { get; set; }

            /// <summary>
            /// Tags to search by (comma separated).
            /// Optional.
            /// </summary>
            string Tags { get; set; }

            /// <summary>
            /// Acceptable values:
            /// MatchAll = 1
            /// MatchAny = 0
            /// Optional.
            /// </summary>
            uint TagType { get; set; }
        }

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
            string Tags_Type { get; set; }
            string Order_By { get; set; }
            string Order_Way { get; set; }
            string Group_Results { get; set; }
            string SearchTerm { get; set; }
            string Page { get; set; }
        }

        // Private

        /// <summary>
        /// Logs a user in to What.CD and stores session cookies.
        /// </summary>
        /// <param name="username">What.CD account username.</param>
        /// <param name="password">What.CD account password.</param>
        private void Login(string username, string password)
        {
            HttpWebRequest Request = WebRequest.Create(new Uri(this.RootWhatCDURI, "login.php")) as HttpWebRequest;
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.CookieContainer = this.CookieJar;
            using (StreamWriter Writer = new StreamWriter(Request.GetRequestStream()))
            {
                Writer.Write(string.Format("username={0}&password={1}", username, password));
            }
            HttpWebResponse Response = Request.GetResponse() as HttpWebResponse;
            if (Response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned. Status code {0}: {1}", Response.StatusCode, Response.StatusDescription));
            Response.Close();
        }

        /// <summary>
        /// Performs a JSON request. 
        /// </summary>
        /// <param name="uri">Base URI.</param>
        /// <param name="request">Request arguments (appended to end of base URI).</param>
        /// <returns>Raw Json results.</returns>
        private string RequestJson(Uri uri, string request)
        {
            HttpWebResponse Response = null;
            string json;
            try
            {
                HttpWebRequest Request = WebRequest.Create(new Uri(uri, request)) as HttpWebRequest;
                Request.ContentType = "application/json; charset=utf-8";
                Request.CookieContainer = this.CookieJar;
                Response = Request.GetResponse() as HttpWebResponse;
                using (StreamReader Reader = new StreamReader(Response.GetResponseStream()))
                {
                    if (Response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned. Status code {0}: {1}", Response.StatusCode, Response.StatusDescription));
                    json = Reader.ReadToEnd();
                }
            }
            finally
            {
                if (Response != null) Response.Close();
            }

            var error = new Regex(@"^\{""status"":""failure"",""error"":""(?<Error>.+)""\}", RegexOptions.IgnoreCase).Matches(json);
            if (error.Count > 0) throw new Exception(error[0].Groups["Error"].Value.ToString().Trim());

            return json;
        }

        /// <summary>
        /// Logs off the current What.CD session.
        /// </summary>
        private void Logoff()
        {
            HttpWebResponse Response = null;
            try
            {
                HttpWebRequest Request = WebRequest.Create(new Uri(this.RootWhatCDURI, string.Format("logout.php?auth={0}", this.GetIndex().response.authkey))) as HttpWebRequest;
                Request.Method = "POST";
                Request.ContentType = "application/x-www-form-urlencoded";
                Request.CookieContainer = this.CookieJar;
                Response = Request.GetResponse() as HttpWebResponse;
                using (StreamReader Reader = new StreamReader(Response.GetResponseStream())) { }
            }
            finally
            {
                if (Response != null) Response.Close();
            }
        }

        /// <summary>
        /// Performs a binary request. 
        /// </summary>
        /// <param name="uri">Base URI.</param>
        /// <param name="request">Request arguments (appended to end of base URI).</param>
        /// <returns>Raw binary results.</returns>
        private byte[] RequestBytes(Uri uri, string request)
        {
            HttpWebResponse Response = null;
            try
            {
                HttpWebRequest Request = WebRequest.Create(new Uri(uri, request)) as HttpWebRequest;
                Request.ContentType = "application/json; charset=utf-8";
                Request.CookieContainer = this.CookieJar;
                Response = Request.GetResponse() as HttpWebResponse;
                using (Stream stream = Response.GetResponseStream())
                {
                    MemoryStream result = new MemoryStream();
                    byte[] buffer = new byte[4096];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        result.Write(buffer, 0, read);
                    }
                    return result.ToArray();
                }
            }
            finally
            {
                if (Response != null) Response.Close();
            }
        }

    }
}
