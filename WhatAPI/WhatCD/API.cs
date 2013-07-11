using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
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

// TODO: test method to validate log checking
// TODO: do a readme!
// TODO: Implement json attributes to change model member name casing
// TODO: Add summary comments for all model properties
// TODO: Double check all escape and unescape locations
// TODO: Make all setting values properties
// TODO: Remove all "this." and rename member valiables to _fieldName standard

namespace WhatCD
{
    /// <summary>
    /// Provides a user-friendly interface to the WhatCD JSON API.
    /// </summary>
    /// <remarks>
    /// Home: https://github.com/frankston/WhatAPI
    /// JSON API Reference: https://what.cd/wiki.php?action=article&id=998
    /// </remarks>
    public class Api : IDisposable
    {

        private Http _http;
        private bool _disposed = false;

        public string AuthKey { get; private set; }

        public Uri RootWhatStatusUri
        {
            get { return _http.BaseWhatStatusUri; }
            set { _http.BaseWhatStatusUri = value; }
        }

        public Uri RootWhatCDUri
        {
            get { return _http.BaseWhatCDUri; }
            set { _http.BaseWhatCDUri = value; }
        }

        /// <summary>
        /// If set to true then the deserialization process will raise an exception if any JSON members exist that do not exist in the model data.
        /// This is primarily intended to be used with integration testing.
        /// </summary>
        public bool ErrorOnMissingMember { get; set; }


        /// <summary>
        /// Logs the user on to what.cd.
        /// </summary>
        /// <param name="username">What.cd username.</param>
        /// <param name="password">What.cd password.</param>
        public Api(string username, string password)
        {
            TaskScheduler.UnobservedTaskException += (s, e) =>
                {
                    Debug.WriteLine("Unobserved task scheduler exception: " + e.Exception.Message);
                    e.SetObserved();
                };
            _http = new Http(username, password);
            this.AuthKey = this.GetIndex().response.authkey;
        }


        // Status

        /// <summary>
        /// Determines if the site, tracker, and irc are up.
        /// </summary>
        /// <returns>JSON response deserialized into Status object.</returns>
        public Status GetStatus()
        {
            var json = _http.RequestJson(this.RootWhatStatusUri, "api/status");
            return Deserialize<Status>(json);
        }

        /// <summary>
        /// Obtains the current site, tracker, and irc uptime (hours).
        /// </summary>
        /// <returns>JSON response deserialized into Uptime object.</returns>
        public Uptime GetUptime()
        {
            var json = _http.RequestJson(this.RootWhatStatusUri, "api/uptime");
            return Deserialize<Uptime>(json);
        }

        /// <summary>
        /// Obtains the maximum uptime records for the site, tracker, and irc (hours).
        /// </summary>
        /// <returns>JSON response deserialized into Records object.</returns>
        public Records GetRecords()
        {
            var json = _http.RequestJson(this.RootWhatStatusUri, "api/records");
            return Deserialize<Records>(json);
        }


        // Forums

        /// <summary>
        /// Get forum scriptions.
        /// </summary>
        /// <param name="onlyShowUnread">Only show subscribed forums with unread threads. Optional.</param>
        /// <returns>JSON response deserialized into Subscriptions object.</returns>
        public Subscriptions GetSubscriptions(bool? onlyShowUnread)
        {
            var builder = new QueryBuilder("ajax.php?action=subscriptions");
            builder.Append("showunread", onlyShowUnread);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<Subscriptions>(json);
        }

        /// <summary>
        /// Gets all forum categories. 
        /// </summary>
        /// <returns>JSON response deserialized into ForumMain object.</returns>
        public ForumMain GetForumMain()
        {
            var json = _http.RequestJson(this.RootWhatCDUri, "ajax.php?action=forum&type=main");
            return Deserialize<ForumMain>(json);
        }

        /// <summary>
        /// Gets a page of threads from a forum.
        /// </summary>
        /// <param name="forumID">Forum ID. Mandatory.</param>
        /// <param name="page">Page number. Optional.</param>
        /// <returns>JSON response deserialized into ForumViewForum object.</returns>
        public ForumViewForum GetForumViewForum(int forumID, int? page)
        {
            var builder = new QueryBuilder("ajax.php?action=forum&type=viewforum");
            builder.Append("forumid", forumID);
            builder.Append("page", page);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<ForumViewForum>(json);
        }

        /// <summary>
        /// Gets information about a particular torrent.
        /// </summary>
        /// <param name="id">ID of torrent. Mandatory.</param>
        /// <returns>JSON response deserialized into ActionTorrent.Torrent object.</returns>
        public WhatCD.Model.ActionTorrent.Torrent GetTorrent(int id)
        {
            var builder = new QueryBuilder("ajax.php?action=torrent");
            builder.Append("id", id);
            return this.GetTorrent(builder);
        }

        /// <summary>
        /// Gets information about a particular torrent.
        /// </summary>
        /// <param name="hash">Hash of torrent. Mandatory.</param>
        /// <returns>JSON response deserialized into ActionTorrent.Torrent object.</returns>
        public WhatCD.Model.ActionTorrent.Torrent GetTorrent(string hash)
        {
            var builder = new QueryBuilder("ajax.php?action=torrent");
            builder.Append("hash", hash);
            return this.GetTorrent(builder);
        }

        /// <summary>
        /// Gets information about a particular torrent.
        /// </summary>
        /// <param name="builder">QueryBuilder object with either a torrent hash or id argument.</param>
        /// <returns>JSON response deserialized into ActionTorrent.Torrent object.</returns>
        private WhatCD.Model.ActionTorrent.Torrent GetTorrent(QueryBuilder builder)
        {
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<WhatCD.Model.ActionTorrent.Torrent>(json);
        }

        /// <summary>
        /// Gets a specific page of forum thread posts.
        /// </summary>
        /// <param name="threadID">Thread ID. Mandatory.</param>
        /// <param name="page">Page number. Optional.</param>
        /// <returns>JSON response deserialized into ForumViewThread object.</returns>
        public ForumViewThread GetForumViewThread(int threadID, int page)
        {
            var builder = new QueryBuilder("ajax.php?action=forum&type=viewthread");
            builder.Append("threadid", threadID);
            builder.Append("page", page);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<ForumViewThread>(json);
        }

        /// <summary>
        /// Gets a page of forum thread posts where a specific thread exists.
        /// </summary>
        /// <param name="postID">Post ID. Mandatory.</param>
        /// <returns>JSON response deserialized into ForumViewThread object.</returns>
        public ForumViewThread GetForumViewThread(int postID)
        {
            var builder = new QueryBuilder("ajax.php?action=forum&type=viewthread");
            builder.Append("postid", postID);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
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
            var builder = new QueryBuilder("ajax.php?action=inbox");
            if (options.DisplayUnreadFirst) builder.Append("sort", "unread");
            builder.Append("type", options.MessageType);
            builder.Append("page", options.Page);
            builder.Append("search", options.SearchTerm);
            builder.Append("searchtype", options.SearchType);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<Inbox>(json);
        }

        /// <summary>
        /// Gets all messages from a conversation.
        /// </summary>
        /// <param name="conversationID">Conversation ID. Mandatory.</param>
        /// <returns>JSON response deserialized into InboxViewConv object.</returns>
        public InboxViewConv GetInboxViewConv(int conversationID)
        {
            var builder = new QueryBuilder("ajax.php?action=inbox&type=viewconv");
            builder.Append("id", conversationID);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<InboxViewConv>(json);
        }


        // Misc

        /// <summary>
        /// Gets information about the current user.
        /// </summary>
        /// <returns>JSON response deserialized into Index object.</returns>
        public Index GetIndex()
        {
            var json = _http.RequestJson(this.RootWhatCDUri, "ajax.php?action=index");
            return Deserialize<Index>(json);
        }

        /// <summary>
        /// Gets the most recent announcements and blog posts.
        /// </summary>
        /// <returns>JSON response deserialized into Announcements object.</returns>
        public Announcements GetAnnouncements()
        {
            var json = _http.RequestJson(this.RootWhatCDUri, "ajax.php?action=announcements");
            return Deserialize<Announcements>(json);
        }

        /// <summary>
        /// Gets notifications.
        /// </summary>
        /// <param name="page">Notification page number. Optional.</param>
        /// <returns>JSON response deserialized into Notifications object.</returns>
        public WhatCD.Model.ActionNotifications.Notifications GetNotifications(int? page)
        {
            var builder = new QueryBuilder("ajax.php?action=notifications");
            builder.Append("page", page);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<WhatCD.Model.ActionNotifications.Notifications>(json);
        }

        /// <summary>
        /// Gets the top torrents.
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, 100, and 250. Optional. Default is 25.</param>
        /// <returns>JSON response deserialized into Top10Torrents object.</returns>
        public Top10Torrents GetTop10Torrents(int? limit)
        {
            var builder = new QueryBuilder("ajax.php?action=top10&type=torrents");
            builder.Append("limit", limit);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<Top10Torrents>(json);
        }

        /// <summary>
        /// Gets the top tags.
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, 100, and 250. Optional. Default is 25.</param>
        /// <returns>JSON response deserialized into Top10Tags object.</returns>
        public Top10Tags GetTop10Tags(int? limit)
        {
            var builder = new QueryBuilder("ajax.php?action=top10&type=tags");
            builder.Append("limit", limit);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<Top10Tags>(json);
        }

        /// <summary>
        /// Gets the top users.
        /// </summary>
        /// <param name="limit">Maximum result limit. Acceptable values: 10, 25, 100, and 250. Optional. Default is 25.</param>
        /// <returns>JSON response deserialized into Top10Users object.</returns>
        public Top10Users GetTop10Users(int? limit)
        {
            var builder = new QueryBuilder("ajax.php?action=top10&type=users");
            builder.Append("limit", limit);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<Top10Users>(json);
        }


        // Requests

        /// <summary>
        /// Gets information about a specific request.
        /// </summary>
        /// <param name="options">Object that inherits IGetRequest</param>
        /// <returns>JSON response deserialized into Request object</returns>
        public WhatCD.Model.ActionRequest.Request GetRequest(IGetRequest options)
        {
            var builder = new QueryBuilder("ajax.php?action=request");
            builder.Append("id", options.RequestID);
            builder.Append("page", options.Page);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<WhatCD.Model.ActionRequest.Request>(json);
        }

        /// <summary>
        /// Searches requests.
        /// If no arguments are specified then the most recent requests are shown.
        /// </summary>
        /// <param name="options">Object that inherits ISearchRequests.</param>
        /// <returns>JSON response deserialized into Requests object.</returns>
        public Requests GetRequests(ISearchRequests options)
        {
            var builder = new QueryBuilder("ajax.php?action=requests");
            builder.Append("tags_type", options.TagsType);
            builder.Append("show_filled", options.ShowFilled.ToString().ToLower());
            builder.Append("page", options.Page);
            builder.Append("tag", options.Tags);
            builder.Append("search", options.SearchTerm);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<Requests>(json);
        }


        // Torrents

        // TODO: Method to get flac log (if it exists)

        /// <summary>
        /// Gets similar artists.
        /// Note: the return response from the server does not conform to the standard JSON response pattern.
        /// </summary>
        /// <param name="artistID">Artist ID. Mandatory.</param>
        /// <param name="limit">Maximum result limit. Mandatory.</param>
        /// <returns>JSON response deserialized into SimilarArtists object.</returns>
        public SimilarArtists GetSimilarArtists(int artistID, int limit)
        {
            var builder = new QueryBuilder("ajax.php?action=similar_artists");
            builder.Append("id", artistID);
            builder.Append("limit", limit);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            // Because similar artists results do not conform to the standard reply we have to be dodgy so it can be deserialised
            if (!string.IsNullOrWhiteSpace(json)) json = string.Format("{{\"artists\":{0}}}", json);
            return Deserialize<SimilarArtists>(json);
        }

        /// <summary>
        /// Searches torrents.
        /// </summary>
        /// <param name="options">Object that inherits from ISearchTorrents.</param>
        /// <returns>JSON response deserialized into Browse object.</returns>
        public Browse GetBrowse(ISearchTorrents options)
        {
            var builder = new QueryBuilder("ajax.php?action=browse");
            builder.Append("artistname", options.ArtistName);
            builder.Append("cataloguenumber", options.CatalogueNumber);
            builder.Append("encoding", options.Encoding);
            builder.Append("filelist", options.FileList);
            builder.Append("format", options.Format);
            builder.Append("freetorrent", options.FreeTorrent);
            builder.Append("group_results", options.GroupResults);
            builder.Append("groupname", options.GroupName);
            builder.Append("hascue", options.HasCue);
            builder.Append("haslog",options.HasLog);
            builder.Append("media", options.Media);
            builder.Append("order_by", options.OrderBy);
            builder.Append("order_way",options.OrderWay);
            builder.Append("page", options.Page);
            builder.Append("recordlabel", options.RecordLabel);
            builder.Append("releasetype", options.ReleaseType);
            builder.Append("remastercataloguenumber", options.RemasterCatalogueNumber);
            builder.Append("remasterrecordlabel", options.RemasterRecordLabel);
            builder.Append("remastertitle", options.RemasterTitle);
            builder.Append("remasteryear", options.RemasterYear);
            builder.Append("scene", options.Scene);
            builder.Append("searchterm", options.SearchTerm);
            builder.Append("taglist", options.TagList);
            builder.Append("tags_type", options.TagsType);
            builder.Append("vanityhouse", options.VanityHouse);
            builder.Append("year", options.Year);

            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<Browse>(json);
        }

        /// <summary>
        /// Gets torrent bookmarks.
        /// </summary>
        /// <returns>JSON response deserialized into BookmarksTorrents object.</returns>
        public BookmarksTorrents GetBookmarksTorrents()
        {
            var json = _http.RequestJson(this.RootWhatCDUri, "ajax.php?action=bookmarks&type=torrents");
            return Deserialize<BookmarksTorrents>(json);
        }

        /// <summary>
        /// Gets artist bookmarks.
        /// </summary>
        /// <returns>JSON response deserialized into BookmarksArtists object.</returns>
        public BookmarksArtists GetBookmarksArtists()
        {
            var json = _http.RequestJson(this.RootWhatCDUri, "ajax.php?action=bookmarks&type=artists");
            return Deserialize<BookmarksArtists>(json);
        }

        /// <summary>
        /// Gets artist information.
        /// </summary>
        /// <param name="artistID">Artist ID. Mandatory.</param>
        /// <returns>JSON response deserialized into Artist object.</returns>
        public WhatCD.Model.ActionArtist.Artist GetArtist(int artistID)
        {
            var builder = new QueryBuilder("ajax.php?action=artist");
            builder.Append("id", artistID);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<WhatCD.Model.ActionArtist.Artist>(json);
        }

        /// <summary>
        /// Gets artist information.
        /// </summary>
        /// <param name="artistName">Artist Name. Mandatory.</param>
        /// <returns>JSON response deserialized into Artist object.</returns>
        public WhatCD.Model.ActionArtist.Artist GetArtist(string artistName)
        {
            var builder = new QueryBuilder("ajax.php?action=artist");
            builder.Append("artistname", artistName);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<WhatCD.Model.ActionArtist.Artist>(json);
        }

        /// <summary>
        /// Gets a torrent group.
        /// </summary>
        /// <param name="groupID">Group ID. Mandatory.</param>
        /// <returns>JSON response deserialized into TorrentGroup object.</returns>
        public TorrentGroup GetTorrentGroup(int groupID)
        {
            var builder = new QueryBuilder("ajax.php?action=torrentgroup");
            builder.Append("id", groupID);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<TorrentGroup>(json);
        }

        /// <summary>
        /// Downloads the torrent file.
        /// </summary>
        /// <param name="torrentID">The ID of the torrent to download. Mandatory.</param>
        /// <returns>A byte array with the torrent file contents.</returns>
        public Torrent DownloadTorrent(int torrentID)
        {
            string contentDisposition;
            string contentType;
            var builder = new QueryBuilder("torrents.php?action=download");
            builder.Append("id", torrentID);
            var bytes = _http.RequestBytes(this.RootWhatCDUri, builder.Query.ToString(), out contentDisposition, out contentType);

            if (!string.Equals(contentType, "application/x-bittorrent; charset=utf-8", StringComparison.InvariantCultureIgnoreCase)) throw new Exception("Expected bittorrent content type was not found");
            if (!contentDisposition.StartsWith("attachment; filename=")) throw new Exception("Torrent filename was not found");

            return new Torrent(bytes, contentDisposition.Replace("attachment; filename=", "").Replace("\"", ""));
        }


        // Users

        /// <summary>
        /// Gets user information.
        /// </summary>
        /// <param name="userID">User ID. Mandatory.</param>
        /// <returns>JSON response deserialized into User object.</returns>
        public User GetUser(int userID)
        {
            var builder = new QueryBuilder("ajax.php?action=user");
            builder.Append("id", userID);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<User>(json);
        }

        /// <summary>
        /// Searches for users.
        /// </summary>
        /// <param name="searchTerm">String to search for. Mandatory.</param>
        /// <param name="page">Results page number. Optional.</param>
        /// <returns>JSON response deserialized into UserSearch object.</returns>
        public UserSearch GetUserSearch(string searchTerm, int? page)
        {
            var builder = new QueryBuilder("ajax.php?action=usersearch");
            builder.Append("search", searchTerm);
            builder.Append("page", page);
            var json = _http.RequestJson(this.RootWhatCDUri, builder.Query.ToString());
            return Deserialize<UserSearch>(json);
        }

        /// <summary>
        /// Determines the rip log score out of 100.
        /// </summary>
        /// <param name="log">Log file contents.</param>
        /// <returns>Score out of 100.</returns>
        public int GetFlacLogScore(string log)
        {
            return _http.GetFlacLogScore(log, this.AuthKey);
        }

        /// <summary>
        /// Logs off the current session.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _http.Logoff(this.AuthKey);
                }
                _disposed = true;
            }
        }

         ~Api()
        {
            Dispose(false);
        }


        // Private

        private T Deserialize<T>(string json)
        {
            var settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = this.ErrorOnMissingMember ? MissingMemberHandling.Error : MissingMemberHandling.Ignore;
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

    }
}