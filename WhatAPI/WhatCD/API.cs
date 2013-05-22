using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using WhatCD.Model;
using WhatCD.Model.ActionAnnouncements;
using WhatCD.Model.ActionBookmarks.TypeArtists;
using WhatCD.Model.ActionBookmarks.TypeTorrents;
using WhatCD.Model.ActionBrowse;
using WhatCD.Model.ActionForum.TypeMain;
using WhatCD.Model.ActionForum.TypeViewForum;
using WhatCD.Model.ActionForum.TypeViewThread;
using WhatCD.Model.ActionInbox;
using WhatCD.Model.ActionInbox.TypeViewConv;
using WhatCD.Model.ActionIndex;
using WhatCD.Model.ActionRequests;
using WhatCD.Model.ActionSimilarArtists;
using WhatCD.Model.ActionSubscriptions;
using WhatCD.Model.ActionTop10.TypeTags;
using WhatCD.Model.ActionTop10.TypeTorrents;
using WhatCD.Model.ActionTop10.TypeUsers;
using WhatCD.Model.ActionTorrentGroup;
using WhatCD.Model.ActionUser;
using WhatCD.Model.ActionUserSearch;
using WhatCD.Model.WhatStatus;

// TODO: Test all forums for invalid dates - I know they're out there!
// TODO: Build in a global timer thread that prevents any call being made to the servers within 2 seconds of any other
// TODO: make download torrent into class (and capture filename)
// TODO: method to capture log file contents
// TODO: Improve comments
// TODO: Add action=torrents
// TODO: Move all bug report info and urls into github
// TODO: Implement json attributes to change model member names to camel casing
// TODO: Tidy up all query building

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

        /// <summary>
        /// If set to true then the deserialization process will raise an exception if any JSON members exist that do not exist in the model data.
        /// This is primarily intended to be used with unit testing.
        /// </summary>
        public bool ErrorOnMissingMember { get; set; }

        /// <summary>
        /// Logs the user on to what.cd.
        /// </summary>
        /// <param name="username">What.cd username.</param>
        /// <param name="password">What.cd password.</param>
        public API(string username, string password)
        {
            this.Login(username, password);
        }


        // Status

        /// <summary>
        /// Determines if the site, tracker, and irc are up.
        /// </summary>
        /// <returns>JSON response deserialized into Status object.</returns>
        public Status GetStatus()
        {
            var json = this.RequestJson(this.RootWhatStatusURI, "api/status");
            return Deserialize<Status>(json);
        }

        /// <summary>
        /// Obtains the current site, tracker, and irc uptime (hours).
        /// </summary>
        /// <returns>JSON response deserialized into Uptime object.</returns>
        public Uptime GetUptime()
        {
            var json = this.RequestJson(this.RootWhatStatusURI, "api/uptime");
            return Deserialize<Uptime>(json);
        }

        /// <summary>
        /// Obtains the maximum uptime records for the site, tracker, and irc (hours).
        /// </summary>
        /// <returns>JSON response deserialized into Records object.</returns>
        public Records GetRecords()
        {
            var json = this.RequestJson(this.RootWhatStatusURI, "api/records");
            return Deserialize<Records>(json);
        }


        // Forums

        /// <summary>
        /// Get forum scriptions.
        /// </summary>
        /// <param name="onlyShowUnread">Only show subscribed forums with unread threads.</param>
        /// <returns>JSON response deserialized into Subscriptions object.</returns>
        public Subscriptions GetSubscriptions(bool onlyShowUnread)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=subscriptions&showunread={0}", onlyShowUnread ? 1 : 0));
            return Deserialize<Subscriptions>(json);
        }

        /// <summary>
        /// Gets all forum categories. 
        /// </summary>
        /// <returns>JSON response deserialized into ForumMain object.</returns>
        public ForumMain GetForumMain()
        {
            var json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=forum&type=main");
            return Deserialize<ForumMain>(json);
        }

        /// <summary>
        /// Gets a page of threads from a forum.
        /// </summary>
        /// <param name="forumID">Forum ID.</param>
        /// <param name="page">Page number.</param>
        /// <returns>JSON response deserialized into ForumViewForum object.</returns>
        public ForumViewForum GetForumViewForum(int forumID, int page)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=forum&type=viewforum&forumid={0}&page={1}", forumID, page));
            return Deserialize<ForumViewForum>(json);
        }

        /// <summary>
        /// Gets a specific page of forum thread posts.
        /// </summary>
        /// <param name="threadID">Thread ID.</param>
        /// <param name="page">Page number.</param>
        /// <returns>JSON response deserialized into ForumViewThread object.</returns>
        public ForumViewThread GetForumViewThread(int threadID, int page)
        {
            return this.SharedGetForumViewThread(string.Format("ajax.php?action=forum&type=viewthread&threadid={0}&page={1}", threadID, page));
        }

        /// <summary>
        /// Gets a page of forum thread posts where a specific thread exists.
        /// </summary>
        /// <param name="postID">Post ID.</param>
        /// <returns>JSON response deserialized into ForumViewThread object.</returns>
        public ForumViewThread GetForumViewThread(int postID)
        {
            return this.SharedGetForumViewThread(string.Format("ajax.php?action=forum&type=viewthread&postid={0}", postID));
        }

        private ForumViewThread SharedGetForumViewThread(string query)
        {
            var json = this.RequestJson(this.RootWhatCDURI, query);
            return Deserialize<ForumViewThread>(json);
        }


        // Messages

        /// <summary>
        /// Searches the contents of messages from either inbox or sentbox.
        /// </summary>
        /// <param name="options">Object implementing the IInbox interface.</param>
        /// <returns>JSON response deserialized into Inbox object.</returns>
        public Inbox GetInbox(IInbox options)
        {
            var query = new StringBuilder();
            query.Append("ajax.php?action=inbox");
            if (options.DisplayUnreadFirst) query.Append("&sort=unread");
            if (!string.IsNullOrWhiteSpace(options.MessageType)) query.Append(string.Format("&type={0}", Uri.EscapeDataString(options.MessageType)));
            if (options.Page != null && options.Page > 0) query.Append(string.Format("&page={0}", options.Page));
            if (!string.IsNullOrWhiteSpace(options.SearchTerm)) query.Append(string.Format("&search={0}", Uri.EscapeDataString(options.SearchTerm)));
            if (!string.IsNullOrWhiteSpace(options.SearchType)) query.Append(string.Format("&searchtype={0}", Uri.EscapeDataString(options.SearchType)));

            var json = this.RequestJson(this.RootWhatCDURI, query.ToString());
            return Deserialize<Inbox>(json);
        }

        /// <summary>
        /// Gets all messages from a conversation.
        /// </summary>
        /// <param name="conversationID">Conversation ID.</param>
        /// <returns>JSON response deserialized into InboxViewConv object.</returns>
        public InboxViewConv GetInboxViewConv(int conversationID)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=inbox&type=viewconv&id={0}", conversationID));
            return Deserialize<InboxViewConv>(json);
        }


        // Misc

        /// <summary>
        /// Gets information about the current user.
        /// </summary>
        /// <returns>JSON response deserialized into Index object.</returns>
        public Index GetIndex()
        {
            var json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=index");
            return Deserialize<Index>(json);
        }

        /// <summary>
        /// Gets what.cd site, irc, and tracker status.
        /// </summary>
        /// <returns>JSON response deserialized into WhatStatus object.</returns>
        public Status GetWhatStatus()
        {
            var json = this.RequestJson(this.RootWhatStatusURI, "json.php");
            return Deserialize<Status>(json);
        }

        /// <summary>
        /// Gets the most recent announcements and blog posts.
        /// </summary>
        /// <returns>JSON response deserialized into Announcements object.</returns>
        public Announcements GetAnnouncements()
        {
            var json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=announcements");
            return Deserialize<Announcements>(json);
        }

        /// <summary>
        /// Gets notifications.
        /// TODO: Issue raised: "currentPages" property should be "currentPage": https://what.cd/forums.php?action=viewthread&threadid=169781
        /// </summary>
        /// <param name="page">Notification page number.</param>
        /// <returns>JSON response deserialized into Notifications object.</returns>
        public WhatCD.Model.ActionNotifications.Notifications GetNotifications(int page)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=notifications&page={0}", page));
            return Deserialize<WhatCD.Model.ActionNotifications.Notifications>(json);
        }

        /// <summary>
        /// Gets the top torrents.
        /// TODO: Null Top10 torrents issue raised: https://what.cd/forums.php?action=viewthread&threadid=169763
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, and 100</param>
        /// <returns>JSON response deserialized into Top10Torrents object.</returns>
        public Top10Torrents GetTop10Torrents(int limit)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=top10&type=torrents&limit={0}", limit));
            return Deserialize<Top10Torrents>(json);
        }

        /// <summary>
        /// Gets the top tags.
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, and 100</param>
        /// <returns>JSON response deserialized into Top10Tags object.</returns>
        public Top10Tags GetTop10Tags(int limit)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=top10&type=tags&limit={0}", limit));
            return Deserialize<Top10Tags>(json);
        }

        /// <summary>
        /// Gets the top users.
        /// TODO: Known null return issue raised: https://what.cd/forums.php?action=viewthread&threadid=169763
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, and 100</param>
        /// <returns>JSON response deserialized into Top10Users object.</returns>
        public Top10Users GetTop10Users(int limit)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=top10&type=users&limit={0}", limit));
            return Deserialize<Top10Users>(json);
        }


        // Requests

        /// <summary>
        /// TODO: Known issue - MusicInfo object sometimes returning as an empty array: https://what.cd/forums.php?action=viewthread&threadid=169790
        /// </summary>
        /// <param name="options">Object that inherits IGetRequest</param>
        /// <returns>JSON response deserialized into Request object</returns>
        public WhatCD.Model.ActionRequest.Request GetRequest(IGetRequest options)
        {
            var query = new StringBuilder();
            query.Append("ajax.php?action=request");
            query.Append(string.Format("&id={0}", options.RequestID));
            if (options.CommentPage != null && options.CommentPage > 0) query.Append(string.Format("&page={0}", options.CommentPage));

            var json = this.RequestJson(this.RootWhatCDURI, query.ToString());
            return Deserialize<WhatCD.Model.ActionRequest.Request>(json);
        }

        /// <summary>
        /// Searches requests.
        /// If no arguments are specified then the most recent requests are shown.
        /// TODO: Known issue - artist array nested in an unnecessary array: https://what.cd/forums.php?action=viewthread&threadid=169787
        /// </summary>
        /// <param name="options">Object that inherits ISearchRequests.</param>
        /// <returns>JSON response deserialized into Requests object.</returns>
        public Requests GetRequests(ISearchRequests options)
        {
            var query = new StringBuilder();
            query.Append(string.Format("ajax.php?action=requests&tags_type={0}&show_filled={1}", options.TagType, options.ShowFilled.ToString().ToLower()));
            if (options.Page > 0) query.Append(string.Format("&page={0}", options.Page));
            if (!string.IsNullOrWhiteSpace(options.Tags)) query.Append(string.Format("&tag={0}", Uri.EscapeDataString(options.Tags)));
            if (!string.IsNullOrWhiteSpace(options.SearchTerm)) query.Append(string.Format("&search={0}", Uri.EscapeDataString(options.SearchTerm)));
            if (!string.IsNullOrWhiteSpace(options.Tags)) query.Append(string.Format("&tags={0}", Uri.EscapeDataString(options.Tags)));

            var json = this.RequestJson(this.RootWhatCDURI, query.ToString());
            return Deserialize<Requests>(json);
        }


        // Torrents

        /// <summary>
        /// Gets similar artists.
        /// TODO: CHECK: Note that the return response from the server is not properly formatted JSON
        /// </summary>
        /// <param name="artistID">Artist ID.</param>
        /// <param name="limit">Maximum result limit.</param>
        /// <returns>JSON response deserialized into SimilarArtists object.</returns>
        public SimilarArtists GetSimilarArtists(int artistID, int limit)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=similar_artists&id={0}&limit={1}", artistID, limit));
            return Deserialize<SimilarArtists>(json);
        }

        /// <summary>
        /// Searches torrents.
        /// </summary>
        /// <param name="options">Object that inherits from ISearchTorrents.</param>
        /// <returns>JSON response deserialized into Browse object.</returns>
        public Browse GetBrowse(ISearchTorrents options)
        {
            var query = new StringBuilder();
            query.Append("ajax.php?action=browse&action=advanced");
            if (!string.IsNullOrWhiteSpace(options.ArtistName)) query.Append(string.Format("&artistname={0}", Uri.EscapeDataString(options.ArtistName)));
            if (!string.IsNullOrWhiteSpace(options.CatalogueNumber)) query.Append(string.Format("&cataloguenumber={0}", Uri.EscapeDataString(options.CatalogueNumber)));
            if (!string.IsNullOrWhiteSpace(options.Encoding)) query.Append(string.Format("&encoding={0}", Uri.EscapeDataString(options.Encoding)));
            if (!string.IsNullOrWhiteSpace(options.FileList)) query.Append(string.Format("&filelist={0}", Uri.EscapeDataString(options.FileList)));
            if (!string.IsNullOrWhiteSpace(options.Format)) query.Append(string.Format("&format={0}", Uri.EscapeDataString(options.Format)));
            if (!string.IsNullOrWhiteSpace(options.FreeTorrent)) query.Append(string.Format("&freetorrent={0}", Uri.EscapeDataString(options.FreeTorrent)));
            if (!string.IsNullOrWhiteSpace(options.GroupResults)) query.Append(string.Format("&group_results={0}", Uri.EscapeDataString(options.GroupResults)));
            if (!string.IsNullOrWhiteSpace(options.GroupName)) query.Append(string.Format("&groupname={0}", Uri.EscapeDataString(options.GroupName)));
            if (!string.IsNullOrWhiteSpace(options.HasCue)) query.Append(string.Format("&hascue={0}", Uri.EscapeDataString(options.HasCue)));
            if (!string.IsNullOrWhiteSpace(options.HasLog)) query.Append(string.Format("&haslog={0}", Uri.EscapeDataString(options.HasLog)));
            if (!string.IsNullOrWhiteSpace(options.Media)) query.Append(string.Format("&media={0}", Uri.EscapeDataString(options.Media)));
            if (!string.IsNullOrWhiteSpace(options.OrderBy)) query.Append(string.Format("&order_by={0}", Uri.EscapeDataString(options.OrderBy)));
            if (!string.IsNullOrWhiteSpace(options.OrderWay)) query.Append(string.Format("&order_way={0}", Uri.EscapeDataString(options.OrderWay)));
            if (!string.IsNullOrWhiteSpace(options.Page)) query.Append(string.Format("&page={0}", Uri.EscapeDataString(options.Page)));
            if (!string.IsNullOrWhiteSpace(options.RecordLabel)) query.Append(string.Format("&recordlabel={0}", Uri.EscapeDataString(options.RecordLabel)));
            if (!string.IsNullOrWhiteSpace(options.ReleaseType)) query.Append(string.Format("&releasetype={0}", Uri.EscapeDataString(options.ReleaseType)));
            if (!string.IsNullOrWhiteSpace(options.RemasterCatalogueNumber)) query.Append(string.Format("&remastercataloguenumber={0}", Uri.EscapeDataString(options.RemasterCatalogueNumber)));
            if (!string.IsNullOrWhiteSpace(options.RemasterRecordLabel)) query.Append(string.Format("&remasterrecordlabel={0}", Uri.EscapeDataString(options.RemasterRecordLabel)));
            if (!string.IsNullOrWhiteSpace(options.RemasterTitle)) query.Append(string.Format("&remastertitle={0}", Uri.EscapeDataString(options.RemasterTitle)));
            if (!string.IsNullOrWhiteSpace(options.RemasterYear)) query.Append(string.Format("&remasteryear={0}", Uri.EscapeDataString(options.RemasterYear)));
            if (!string.IsNullOrWhiteSpace(options.Scene)) query.Append(string.Format("&scene={0}", Uri.EscapeDataString(options.Scene)));
            if (!string.IsNullOrWhiteSpace(options.SearchTerm)) query.Append(string.Format("&searchterm={0}", Uri.EscapeDataString(options.SearchTerm)));
            if (!string.IsNullOrWhiteSpace(options.TagList)) query.Append(string.Format("&taglist={0}", Uri.EscapeDataString(options.TagList)));
            if (!string.IsNullOrWhiteSpace(options.TagsType)) query.Append(string.Format("&tags_type={0}", Uri.EscapeDataString(options.TagsType)));
            if (!string.IsNullOrWhiteSpace(options.VanityHouse)) query.Append(string.Format("&vanityhouse={0}", Uri.EscapeDataString(options.VanityHouse)));
            if (!string.IsNullOrWhiteSpace(options.Year)) query.Append(string.Format("&year={0}", Uri.EscapeDataString(options.Year)));

            var json = this.RequestJson(this.RootWhatCDURI, string.Format(query.ToString()));
            return Deserialize<Browse>(json);
        }

        /// <summary>
        /// Gets torrent bookmarks.
        /// </summary>
        /// <returns>JSON response deserialized into BookmarksTorrents object.</returns>
        public BookmarksTorrents GetBookmarksTorrents()
        {
            var json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=bookmarks&type=torrents");
            return Deserialize<BookmarksTorrents>(json);
        }

        /// <summary>
        /// Gets artist bookmarks.
        /// </summary>
        /// <returns>JSON response deserialized into BookmarksArtists object.</returns>
        public BookmarksArtists GetBookmarksArtists()
        {
            var json = this.RequestJson(this.RootWhatCDURI, "ajax.php?action=bookmarks&type=artists");
            return Deserialize<BookmarksArtists>(json);
        }

        /// <summary>
        /// Gets artist information.
        /// TODO: Known Issue: Only populates similar artists the first time an artist is queried and never again: https://what.cd/forums.php?action=viewthread&threadid=169786 
        /// </summary>
        /// <param name="artistID">Artist ID.</param>
        /// <returns>JSON response deserialized into Artist object.</returns>
        public WhatCD.Model.ActionBrowse.Artist GetArtist(int artistID)
        {
            return this.SharedGetArtist(string.Format("ajax.php?action=artist&id={0}", artistID));
        }

        /// <summary>
        /// Gets artist information.
        /// TODO: Known Issue: Only populates similar artists the first time an artist is queried and never again: https://what.cd/forums.php?action=viewthread&threadid=169786 
        /// </summary>
        /// <param name="artistName">Artist Name.</param>
        /// <returns>JSON response deserialized into Artist object.</returns>
        public WhatCD.Model.ActionBrowse.Artist GetArtist(string artistName)
        {
            return this.SharedGetArtist(string.Format("ajax.php?action=artist&artistname={0}", Uri.EscapeDataString(artistName)));
        }

        private WhatCD.Model.ActionBrowse.Artist SharedGetArtist(string query)
        {
            var json = this.RequestJson(this.RootWhatCDURI, query);
            return Deserialize<WhatCD.Model.ActionBrowse.Artist>(json);
        }

        /// <summary>
        /// Gets a torrent group.
        /// TODO: Known HTML response issue raised: https://what.cd/forums.php?action=viewthread&threadid=169772
        /// </summary>
        /// <param name="groupID">Group ID.</param>
        /// <returns>JSON response deserialized into TorrentGroup object.</returns>
        public TorrentGroup GetTorrentGroup(int groupID)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=torrentgroup&id={0}", groupID));
            return Deserialize<TorrentGroup>(json);
        }

        /// <summary>
        /// Downloads the torrent file.
        /// </summary>
        /// <param name="torrentID">The ID of the torrent to download</param>
        /// <returns>A byte array with the torrent file contents.</returns>
        public byte[] DownloadTorrent(int torrentID)
        {
            return this.RequestBytes(this.RootWhatCDURI, string.Format("torrents.php?action=download&id={0}", torrentID));
        }


        // Users

        /// <summary>
        /// Gets user information.
        /// TODO: Known null response issue raised: https://what.cd/forums.php?action=viewthread&threadid=169775
        /// TODO: Known invalid datetime issue: https://what.cd/forums.php?action=viewthread&threadid=169791
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <returns>JSON response deserialized into User object.</returns>
        public User GetUser(int userID)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=user&id={0}", userID));
            return Deserialize<User>(json);
        }

        /// <summary>
        /// Searches for users.
        /// </summary>
        /// <param name="searchTerm">String to search for.</param>
        /// <param name="page">Results page number.</param>
        /// <returns>JSON response deserialized into UserSearch object.</returns>
        public UserSearch GetUserSearch(string searchTerm, int page)
        {
            var json = this.RequestJson(this.RootWhatCDURI, string.Format("ajax.php?action=usersearch&search={0}&page={1}", Uri.EscapeDataString(searchTerm), page));
            return Deserialize<UserSearch>(json);
        }

        /// <summary>
        /// Determines the rip log score out of 100.
        /// </summary>
        /// <param name="log">Log file contents.</param>
        /// <returns>Score out of 100.</returns>
        public int GetFlacLogScore(string log)
        {
            HttpWebResponse response = null;
            try
            {
                log = HttpUtility.UrlEncode(log);
                var request = WebRequest.Create(new Uri(this.RootWhatCDURI, "logchecker.php")) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = this.CookieJar;

                using (var writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write("action=takeupload&auth={0}&log_contents={1}", this.GetIndex().response.authkey, log);
                }
                response = request.GetResponse() as HttpWebResponse;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    if (response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned. Status code {0}: {1}", response.StatusCode, response.StatusDescription));

                    var regex = new Regex(@">(?<score>[-\d]+)</span> \(out of 100\)</blockquote>");
                    var htmlResponse = reader.ReadToEnd();
                    var matches = regex.Matches(htmlResponse);
                    if (matches.Count != 1) throw new WebException("Score not found");
                    int score;
                    if (!int.TryParse(matches[0].Groups["score"].Value.ToString(), out score)) throw new Exception("Failed to convert score to int.");
                    return score;
                }
            }
            finally
            {
                response.Close();
            }
        }

        /// <summary>
        /// Logs off the current session.
        /// </summary>
        public void Dispose()
        {
            this.Logoff();
        }


        // Private

        private T Deserialize<T>(string json)
        {
            var settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = this.ErrorOnMissingMember ? MissingMemberHandling.Error : MissingMemberHandling.Ignore;
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// Logs a user in to What.CD and stores session cookies.
        /// </summary>
        /// <param name="username">What.CD account username.</param>
        /// <param name="password">What.CD account password.</param>
        private void Login(string username, string password)
        {
            var request = WebRequest.Create(new Uri(this.RootWhatCDURI, "login.php")) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = this.CookieJar;
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(string.Format("username={0}&password={1}", Uri.EscapeDataString(username), Uri.EscapeDataString(password)));
            }
            var response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned. Status code {0}: {1}", response.StatusCode, response.StatusDescription));
            response.Close();
        }

        /// <summary>
        /// Performs a JSON request. 
        /// </summary>
        /// <param name="uri">Base URI.</param>
        /// <param name="query">Request arguments (appended to end of base URI).</param>
        /// <returns>Raw Json results.</returns>
        private string RequestJson(Uri uri, string query)
        {
            HttpWebResponse response = null;
            string json;
            try
            {
                var request = WebRequest.Create(new Uri(uri, query)) as HttpWebRequest;
                request.ContentType = "application/json; charset=utf-8";
                request.CookieContainer = this.CookieJar;
                response = request.GetResponse() as HttpWebResponse;
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    if (response.StatusCode != HttpStatusCode.OK) throw new WebException(string.Format("Non-OK HTTP status returned. Status code {0}: {1}", response.StatusCode, response.StatusDescription));
                    json = reader.ReadToEnd();
                }
            }
            finally
            {
                if (response != null) response.Close();
            }
            // Throw exception if response is of standard error format
            var error = new Regex(@"^\{""status"":""failure"",""error"":""(?<Error>.+)""\}", RegexOptions.IgnoreCase).Matches(json);
            if (error.Count > 0) throw new Exception(error[0].Groups["Error"].Value.ToString().Trim());

            return json;
        }

        /// <summary>
        /// Logs off the current What.CD session.
        /// </summary>
        private void Logoff()
        {
            HttpWebResponse response = null;
            try
            {
                var request = WebRequest.Create(new Uri(this.RootWhatCDURI, string.Format("logout.php?auth={0}", this.GetIndex().response.authkey))) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = this.CookieJar;
                response = request.GetResponse() as HttpWebResponse;
                using (var reader = new StreamReader(response.GetResponseStream())) { }
            }
            finally
            {
                if (response != null) response.Close();
            }
        }

        /// <summary>
        /// Performs a binary request. 
        /// </summary>
        /// <param name="uri">Base URI.</param>
        /// <param name="query">Request arguments (appended to end of base URI).</param>
        /// <returns>Raw binary results.</returns>
        private byte[] RequestBytes(Uri uri, string query)
        {
            HttpWebResponse response = null;
            try
            {
                var request = WebRequest.Create(new Uri(uri, query)) as HttpWebRequest;
                request.ContentType = "application/json; charset=utf-8";
                request.CookieContainer = this.CookieJar;
                response = request.GetResponse() as HttpWebResponse;
                using (var stream = response.GetResponseStream())
                {
                    var result = new MemoryStream();
                    var buffer = new byte[4096];
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
                if (response != null) response.Close();
            }
        }
    }
}