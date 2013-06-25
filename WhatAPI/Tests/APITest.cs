using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatCD;
using WhatCD.Model;
using WhatCD.Model.ActionAnnouncements;
using System.Web;

namespace Tests
{
    [TestClass]
    public class ApiTest
    {

        private static Api Api;
        private static WhatCD.Random WhatRandom;
        private const int MAX_RANDOM_RANGE_SIZE = 20;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Api = new Api("YourUsername", "YourPassword") { ErrorOnMissingMember = true };
            WhatRandom = new WhatCD.Random(Api);
        }

        [TestMethod]
        public void GetSubscriptionsTest()
        {
            var subscriptions = Api.GetSubscriptions(true);
            this.PerformCommonResponseTests(subscriptions);
        }

        [TestMethod]
        public void GetForumMainTest()
        {
            var forumMain = Api.GetForumMain();
            this.PerformCommonResponseTests(forumMain);
        }

        [TestMethod]
        public void GetForumViewForumTest()
        {
            var forumID = WhatRandom.GetForumId();

            // First page
            TestContext.WriteLine("Requesting and testing page {0} of forum ID {1}...", 1, forumID);
            var forumPage1 = Api.GetForumViewForum(forumID, 1);
            this.PerformCommonResponseTests(forumPage1);
            TestContext.WriteLine("Passed.");

            // Random page
            if (forumPage1.response.pages > 1)
            {
                var page = WhatCD.Random.GetRandomIntFromRange(2, forumPage1.response.pages);
                TestContext.WriteLine("Requesting and testing (random) page {0} of forum ID {1}...", page, forumID);
                var forumPageX = Api.GetForumViewForum(forumID, page);
                this.PerformCommonResponseTests(forumPageX);
                TestContext.WriteLine("Passed.");
            }
        }

        [TestMethod]
        public void GetForumViewThreadTest()
        {
            var threadId = WhatRandom.GetForumThreadId();

            // First page
            TestContext.WriteLine("Requesting and testing page {0} of thread ID {1}...", 1, threadId);
            var threadPage1 = Api.GetForumViewThread(threadId, 1);
            this.PerformCommonResponseTests(threadPage1);
            TestContext.WriteLine("Passed.");

            // Random page
            if (threadPage1.response.pages > 1)
            {
                var page = WhatCD.Random.GetRandomIntFromRange(2, threadPage1.response.pages);
                TestContext.WriteLine("Requesting and testing (random) page {0} of thread ID {1}...", page, threadId);
                var threadPageX = Api.GetForumViewThread(threadId, page);
                this.PerformCommonResponseTests(threadPageX);
                TestContext.WriteLine("Passed.");
            }
        }

        [TestMethod]
        public void GetInboxTest_Inbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "inbox" }, false);
        }

        [TestMethod]
        public void GetInboxTest_Sentbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "sentbox" }, false);
        }

        [TestMethod]
        public void GetInboxViewConvTest_Inbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "inbox" }, true);
        }

        [TestMethod]
        public void GetInboxViewConvTest_Sentbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "sentbox" }, true);
        }

        private void InboxAndConversationCommon(SearchInbox searchOptions, bool viewConversation)
        {

            // First page
            TestContext.WriteLine("Requesting and testing page {0} of {1}...", 1, searchOptions.MessageType);
            searchOptions.Page = 1;
            var inboxPage1 = Api.GetInbox(searchOptions);
            this.PerformCommonResponseTests(inboxPage1);
            TestContext.WriteLine("Passed.");

            // Random page
            if (inboxPage1.response.pages > 1)
            {
                searchOptions.Page = WhatCD.Random.GetRandomIntFromRange(2, inboxPage1.response.pages);
                TestContext.WriteLine("Requesting and testing (random) page {0} of {1}...", searchOptions.Page, searchOptions.MessageType);
                var inboxPageX = Api.GetInbox(searchOptions);
                this.PerformCommonResponseTests(inboxPageX);
                TestContext.WriteLine("Passed.");
            }

            // Conversations
            if (viewConversation)
            {
                for (int i = 0; i < inboxPage1.response.messages.Count; i++)
                {
                    TestContext.WriteLine("Requesting and testing conversation {0} of {1}...", i + 1, inboxPage1.response.messages.Count);
                    var conversation = Api.GetInboxViewConv(inboxPage1.response.messages[i].convId);
                    this.PerformCommonResponseTests(conversation);
                    TestContext.WriteLine("Passed.");
                }
            }
        }

        [TestMethod]
        public void GetIndexTest()
        {
            var index = Api.GetIndex();
            this.PerformCommonResponseTests(index);
        }

        [TestMethod]

        public void GetStatusTest()
        {
            var status = Api.GetStatus();
            Assert.IsNotNull(status);
        }

        [TestMethod]
        public void GetUptimeTest()
        {
            var uptime = Api.GetUptime();
            Assert.IsNotNull(uptime);
        }

        [TestMethod]
        public void GetRecordsTest()
        {
            var records = Api.GetRecords();
            Assert.IsNotNull(records);
        }

        [TestMethod]
        public void GetAnnouncementsTest()
        {
            var announcements = Api.GetAnnouncements();
            this.PerformCommonResponseTests(announcements);
        }

        [TestMethod]
        public void GetNotificationsTest()
        {
            // First page
            TestContext.WriteLine("Requesting and testing notifications page {0}...", 1);
            var notificationsPage1 = Api.GetNotifications(1);
            this.PerformCommonResponseTests(notificationsPage1);
            TestContext.WriteLine("Passed.");

            // Random page
            if (notificationsPage1.response.pages > 1)
            {
                var page = WhatCD.Random.GetRandomIntFromRange(2, notificationsPage1.response.pages);
                TestContext.WriteLine("Requesting and testing notifications page {0} of {1}...", page, notificationsPage1.response.pages);
                var notificationsPageX = Api.GetNotifications(page);
                this.PerformCommonResponseTests(notificationsPageX);
                TestContext.WriteLine("Passed.");
            }
        }

        [TestMethod]
        public void GetTop10TorrentsTest_Limit10()
        {
            var top10Torrents = Api.GetTop10Torrents(10);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        public void GetTop10TorrentsTest_Limit25()
        {
            var top10Torrents = Api.GetTop10Torrents(25);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        public void GetTop10TorrentsTest_Limit100()
        {
            var top10Torrents = Api.GetTop10Torrents(100);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        public void GetTop10TorrentsTest_Limit250()
        {
            var top10Torrents = Api.GetTop10Torrents(250);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        public void GetTop10TagsTest_Limit10()
        {
            var top10Tags = Api.GetTop10Tags(10);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        public void GetTop10TagsTest_Limit25()
        {
            var top10Tags = Api.GetTop10Tags(25);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        public void GetTop10TagsTest_Limit100()
        {
            var top10Tags = Api.GetTop10Tags(100);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        public void GetTop10TagsTest_Limit250()
        {
            var top10Tags = Api.GetTop10Tags(250);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        public void GetTop10UsersTest_Limit10()
        {
            var top10Users = Api.GetTop10Users(10);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        public void GetTop10UsersTest_Limit25()
        {
            var top10Users = Api.GetTop10Users(25);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        public void GetTop10UsersTest_Limit100()
        {
            var top10Users = Api.GetTop10Users(100);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        public void GetTop10UsersTest_Limit250()
        {
            var top10Users = Api.GetTop10Users(250);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        public void GetTorrentTest_ById()
        {
            TestContext.WriteLine("Finding random torrent ID...");
            var torrentID = WhatRandom.GetTorrentId();
            TestContext.WriteLine("Done. Found random torrent ID ({0}). Getting torrent information...", torrentID);
            var torrent = Api.GetTorrent(torrentID);
            this.PerformCommonResponseTests(torrent);
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void GetTorrentTest_ByHash()
        {
            TestContext.WriteLine("Finding random torrent hash...");
            var torrentHash = WhatRandom.GetTorrentHash();
            TestContext.WriteLine("Done. Found random torrent hash ({0}). Getting torrent information...", torrentHash);
            var torrent = Api.GetTorrent(torrentHash);
            this.PerformCommonResponseTests(torrent);
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void GetRequestsTest()
        {
            TestContext.WriteLine("Finding random page of requests...");
            var requests = WhatRandom.GetRequests();
            TestContext.WriteLine("Done. Randomly selected requests page {0}. Validating requests page...", requests.response.currentPage);
            this.PerformCommonResponseTests(requests);
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void GetSimilarArtistsTest()
        {
            TestContext.WriteLine("Finding random artist that has similar artists...");
            var artist = WhatRandom.GetIdOfArtistThatHasSimilarArtists();
            TestContext.WriteLine("Done. ID of artist that has similar artists is {0}. Validating similar artists...", artist);
            var similarArtists = Api.GetSimilarArtists(artist, 100);
            for (int i = 0; i < similarArtists.artists.Count; i++)
            {
                Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(similarArtists.artists[i]), "Similar artist ({0}) info is null", similarArtists.artists[i]);
            }
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void GetBrowseTest()
        {
            // First page
            var browseOptions = new SearchTorrents() { GroupName = WhatCD.Random.RandomCharString(1) };
            TestContext.WriteLine("Requesting and testing browse page {0} of GroupName search criteria '{1}'...", 1, browseOptions.GroupName);
            var browsePage1 = Api.GetBrowse(browseOptions);
            this.PerformCommonResponseTests(browsePage1);
            TestContext.WriteLine("Passed.");

            // Random page
            if (browsePage1.response.pages > 1)
            {
                browseOptions.Page = WhatCD.Random.GetRandomIntFromRange(2, browsePage1.response.pages);
                TestContext.WriteLine("Requesting and testing browse page {0} of {1}...", browseOptions.Page, browsePage1.response.pages);
                var browsePageX = Api.GetBrowse(browseOptions);
                this.PerformCommonResponseTests(browsePageX);
                TestContext.WriteLine("Passed.");
            }
        }

        [TestMethod]
        public void GetBookmarksTorrentsTest()
        {
            var bookmarksTorrents = Api.GetBookmarksTorrents();
            this.PerformCommonResponseTests(bookmarksTorrents);
        }

        [TestMethod]
        public void GetBookmarksArtistsTest()
        {
            var bookmarksArtists = Api.GetBookmarksArtists();
            this.PerformCommonResponseTests(bookmarksArtists);
        }

        [TestMethod]
        public void GetArtistTest_ByName()
        {
            TestContext.WriteLine("Searching for random artist name...");
            var artistName = WhatRandom.GetArtistName();
            TestContext.WriteLine("Done. Found artist '{0}'. Requesting and validating artist information...", artistName);
            var artist = Api.GetArtist(artistName);
            this.PerformCommonResponseTests(artist);
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void GetArtistTest_ById()
        {
            TestContext.WriteLine("Searching for random artist id...");
            var artistId = WhatRandom.GetArtistId();
            TestContext.WriteLine("Done. Found artist id '{0}'. Requesting and validating artist information...", artistId);
            var artist = Api.GetArtist(artistId);
            this.PerformCommonResponseTests(artist);
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void GetTorrentGroupTest()
        {
            TestContext.WriteLine("Searching for random group id...");
            var groupId = WhatRandom.GetGroupId();
            TestContext.WriteLine("Done. Found group id '{0}'. Requesting and validating group information...", groupId);
            var groups = Api.GetTorrentGroup(groupId);
            this.PerformCommonResponseTests(groups);
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void DownloadTorrentTest()
        {
            TestContext.WriteLine("Searching for random valid torrent id...");
            var torrentId = WhatRandom.GetTorrentId();
            TestContext.WriteLine("Done. Found torrent id '{0}'. Downloading and validating torrent file...", torrentId);
            var torrent = Api.DownloadTorrent(torrentId);
            Assert.IsTrue(torrent != null && torrent.Length > 0, "Torrent download data is null or empty");
            TestContext.WriteLine("Passed.");
        }

        [TestMethod]
        public void GetUserTest()
        {
            TestContext.WriteLine("Searching for random valid user id...");
            var userId = WhatRandom.GetUserId();
            TestContext.WriteLine("Done. Found user id '{0}'. Getting and validating user information...", userId);
            var user = Api.GetUser(userId);
            this.PerformCommonResponseTests(user);
            TestContext.WriteLine("Passed");
        }

        [TestMethod]
        public void GetUserSearchTest()
        {
            // First page
            string searchTerm = WhatCD.Random.RandomCharString(1);
            TestContext.WriteLine("Requesting and testing page {0} of user search '{1}'...", 1, searchTerm);
            var searchPage1 = Api.GetUserSearch(searchTerm, null);
            this.PerformCommonResponseTests(searchPage1);
            TestContext.WriteLine("Passed.");

            // Random page
            if (searchPage1.response.pages > 1)
            {
                var page = WhatCD.Random.GetRandomIntFromRange(2, searchPage1.response.pages);
                TestContext.WriteLine("Requesting and testing search page {0} of {1}...", page, searchPage1.response.pages);
                var searchPageX = Api.GetUserSearch(searchTerm, page);
                this.PerformCommonResponseTests(searchPageX);
                TestContext.WriteLine("Passed.");
            }
        }

        // TODO: GetFlacLogScore

        private void PerformCommonResponseTests<T>(IResponse<T> deserializedJson)
        {
            Assert.IsNotNull(deserializedJson, "Response object is null");
            StringAssert.Matches(deserializedJson.status, new Regex("success", RegexOptions.IgnoreCase), string.Format("Unexpected response status ({0})", deserializedJson.status));
            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(deserializedJson.response), "JSON response data is null");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Api.Dispose();
        }

        private class SearchInbox : IInbox
        {
            public string MessageType { get; set; }
            public bool DisplayUnreadFirst { get; set; }
            public int? Page { get; set; }
            public string SearchType { get; set; }
            public string SearchTerm { get; set; }
        }

        private class SearchTorrents : ISearchTorrents
        {
            public string ArtistName { get; set; }
            public string GroupName { get; set; } // torrent name
            public string RecordLabel { get; set; }
            public string CatalogueNumber { get; set; }
            public int? Year { get; set; }
            public string RemasterTitle { get; set; }
            public string RemasterYear { get; set; }
            public string RemasterRecordLabel { get; set; }
            public string RemasterCatalogueNumber { get; set; }
            public string FileList { get; set; }
            public string Encoding { get; set; }
            public string Format { get; set; }
            public string Media { get; set; }
            public string ReleaseType { get; set; }
            public bool? HasLog { get; set; }
            public bool? HasCue { get; set; }
            public bool? Scene { get; set; }
            public bool? VanityHouse { get; set; }
            public string FreeTorrent { get; set; }
            public string TagList { get; set; }
            public string TagsType { get; set; }
            public string OrderBy { get; set; }
            public string OrderWay { get; set; }
            public string GroupResults { get; set; }
            public string SearchTerm { get; set; }
            public int? Page { get; set; }
        }

    }
}