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

namespace Tests
{
    [TestClass]
    public class APITest
    {

        private static API Api;
        private static int RandomRangeItems = 20;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Api = new API("yourOwnUsername", "yourOwnPassword") { ErrorOnMissingMember = true };
        }

        [TestMethod]
        [Description("Verifies GetSubscriptions method returns successfully and that response object contains at least one non-default property value")]
        public void GetSubscriptionsTest()
        {
            System.Threading.Thread.Sleep(2000);
            var subscriptions = Api.GetSubscriptions(true);

            this.PerformCommonResponseTests(subscriptions);
        }

        [TestMethod]
        [Description("Verifies GetForumMain method returns successfully and that response object contains at least one non-default property value")]
        public void GetForumMainTest()
        {
            System.Threading.Thread.Sleep(2000);
            var forumMain = Api.GetForumMain();

            this.PerformCommonResponseTests(forumMain);
        }

        [TestMethod]
        [Description("Verifies GetForumViewForum method returns successfully and that response object contains at least one non-default property value. Walks all available pages")]
        public void GetForumViewForumTest()
        {
            int[] validForumIDs = new int[] { 7, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 23, 25, 26, 27, 29, 32, 36, 37, 40, 43, 45, 55, 56, 61, 62, 63, 65 };
            var forumID = Helper.GetRandomElementFromArray(validForumIDs);

            TestContext.WriteLine("Forum ID: " + forumID);

            int currentPage = 0;
            int totalPages;

            do
            {
                System.Threading.Thread.Sleep(2000);
                currentPage++;
                var forum = Api.GetForumViewForum(forumID, currentPage);

                this.PerformCommonResponseTests(forum);

                totalPages = forum.response.pages;
                TestContext.WriteLine("Page {0} of {1} pass ok.", currentPage, totalPages);

            } while (currentPage != totalPages);
        }

        [TestMethod]
        [Description("Verifies ForumViewForumThread method returns successfully with a random forum ID between 2000 and 65000 and walks all available pages within the specific forum")]
        public void GetForumViewThreadTest()
        {
            var forumID = Helper.GetRandomIntFromRange(2000, 65000);
            TestContext.WriteLine("Forum ID: " + forumID);

            int currentPage = 0;
            int totalPages;

            do
            {
                System.Threading.Thread.Sleep(2000);
                currentPage++;
                var forum = Api.GetForumViewThread(forumID, currentPage);

                this.PerformCommonResponseTests(forum);

                totalPages = forum.response.pages;
                TestContext.WriteLine("Page {0} of {1} pass ok.", currentPage, totalPages);

            } while (currentPage != totalPages);
        }

        [TestMethod]
        [Description("Verifies GetInbox method returns successfully. Tests all pages of messages in the inbox.")]
        public void GetInboxTest_Inbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "inbox" }, false);
        }

        [TestMethod]
        [Description("Verifies GetInbox method returns successfully. Tests all pages of messages in the sentbox.")]
        public void GetInboxTest_Sentbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "sentbox" }, false);
        }
     
        [TestMethod]
        [Description("Verifies GetInboxViewConv method returns successfully. Tests all conversations within all pages of messages in the inbox.")]
        public void GetInboxViewConvTest_Inbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "inbox" }, true);
        }

        [TestMethod]
        [Description("Verifies GetInboxViewConv method returns successfully. Tests all conversations within all pages of messages in the sentbox.")]
        public void GetInboxViewConvTest_Sentbox()
        {
            this.InboxAndConversationCommon(new SearchInbox() { MessageType = "sentbox" }, true);
        }

        private void InboxAndConversationCommon(SearchInbox searchOptions, bool viewConversation)
        {
            int totalPages;
            searchOptions.Page = 0;

            do
            {
                System.Threading.Thread.Sleep(2000);
                searchOptions.Page++;
                var inbox = Api.GetInbox(searchOptions);

                this.PerformCommonResponseTests(inbox);

                totalPages = inbox.response.pages;
                TestContext.WriteLine("{0} page {1} of {2} pass ok.", searchOptions.MessageType, searchOptions.Page, totalPages);

                if (viewConversation)
                {
                    for (int i = 0; i < inbox.response.messages.Count; i++)
                    {
                        System.Threading.Thread.Sleep(2000);
                        var conversation = Api.GetInboxViewConv(inbox.response.messages[i].convId);
                        this.PerformCommonResponseTests(conversation);
                        TestContext.WriteLine("{0} conversation {1} of {2} pass ok.", searchOptions.MessageType, i + 1, inbox.response.messages.Count);
                    }
                }

            } while (searchOptions.Page != totalPages);
        }

        [TestMethod]
        [Description("Verifies GetIndex method returns successfully and that response object contains at least one non-default property value")]
        public void GetIndexTest()
        {
            System.Threading.Thread.Sleep(2000);
            var index = Api.GetIndex();

            this.PerformCommonResponseTests(index);
        }

        [TestMethod]
        [Description("Verifies GetStatus method returns a non-null object")]
        public void GetStatusTest()
        {
            System.Threading.Thread.Sleep(2000);
            var status = Api.GetStatus();

            Assert.IsNotNull(status);
        }

        [TestMethod]
        [Description("Verifies GetUptime method returns a non-null object")]
        public void GetUptimeTest()
        {
            System.Threading.Thread.Sleep(2000);
            var uptime = Api.GetUptime();

            Assert.IsNotNull(uptime);
        }

        [TestMethod]
        [Description("Verifies GetRecords method returns a non-null object")]
        public void GetRecordsTest()
        {
            System.Threading.Thread.Sleep(2000);
            var records = Api.GetRecords();

            Assert.IsNotNull(records);
        }

        [TestMethod]
        [Description("Verifies GetAnnouncements method returns successfully and that response object contains at least one non-default property value")]
        public void GetAnnouncementsTest()
        {
            System.Threading.Thread.Sleep(2000);
            var announcements = Api.GetAnnouncements();

            this.PerformCommonResponseTests(announcements);
        }

        [TestMethod]
        [Description("Verifies GetNotifications method returns successfully and that response object contains at least one non-default property value. Walks all available pages")]
        public void GetNotificationsTest()
        {
            int currentPage = 0;
            int totalPages;

            do
            {
                System.Threading.Thread.Sleep(2000);
                currentPage++;
                var notifications = Api.GetNotifications(currentPage);

                this.PerformCommonResponseTests(notifications);

                totalPages = notifications.response.pages;
                TestContext.WriteLine("Page {0} of {1} pass ok.", currentPage, totalPages);

            } while (currentPage != totalPages);
        }

        [TestMethod]
        [Description("Verifies GetTop10Torrents method (with a limit of 10) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TorrentsTest_Limit10()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Torrents = Api.GetTop10Torrents(10);

            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        [Description("Verifies GetTop10Torrents method (with a limit of 25) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TorrentsTest_Limit25()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Torrents = Api.GetTop10Torrents(25);

            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        [Description("Verifies GetTop10Torrents method (with a limit of 100) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TorrentsTest_Limit100()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Torrents = Api.GetTop10Torrents(100);

            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        [Description("Verifies GetTop10Tags method (with a limit of 10) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TagsTest_Limit10()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Tags = Api.GetTop10Tags(10);

            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        [Description("Verifies GetTop10Tags method (with a limit of 25) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TagsTest_Limit25()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Tags = Api.GetTop10Tags(25);

            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        [Description("Verifies GetTop10Tags method (with a limit of 100) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TagsTest_Limit100()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Tags = Api.GetTop10Tags(100);

            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        [Description("Verifies GetTop10Users method (with a limit of 10) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10UsersTest_Limit10()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Users = Api.GetTop10Users(10);

            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        [Description("Verifies GetTop10Users method (with a limit of 25) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10UsersTest_Limit25()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Users = Api.GetTop10Users(25);

            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        [Description("Verifies GetTop10Users method (with a limit of 100) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10UsersTest_Limit100()
        {
            System.Threading.Thread.Sleep(2000);
            var top10Users = Api.GetTop10Users(100);

            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        [Description("Verifies that a randomly selected range of pages validate successfully")]
        public void GetRequestsTest()
        {

            // Get first pages of requests
            SearchRequests searchArgs = new SearchRequests() { ShowFilled = true};
            System.Threading.Thread.Sleep(2000);
            var requests = Api.GetRequests(searchArgs);
            this.PerformCommonResponseTests(requests);

            // Define a random page range
            int firstPage;
            int lastPage;
            if (requests.response.pages > RandomRangeItems)
            {
                firstPage = Helper.GetRandomIntFromRange(1, requests.response.pages - RandomRangeItems);
                lastPage = firstPage + RandomRangeItems;
            }
            else
            {
                firstPage = 1;
                lastPage = requests.response.pages;
            }

            // Loop through page range
            for (int i = firstPage; i < lastPage; i++)
            {
                System.Threading.Thread.Sleep(2000);
                searchArgs.Page = i;
                var requestsSearch = Api.GetRequests(searchArgs);

                this.PerformCommonResponseTests(requestsSearch);
                TestContext.WriteLine("Page {0} of {1} pass ok.", i, requestsSearch.response.pages);
            }
        }

        // TODO: GetRequests

        // TODO: GetSimilarArtists

        // TODO: GetBrowse

        [TestMethod]
        [Description("Verifies GetBookmarksTorrents method returns successfully and that response object contains at least one non-default property value")]
        public void GetBookmarksTorrentsTest()
        {
            System.Threading.Thread.Sleep(2000);
            var bookmarksTorrents = Api.GetBookmarksTorrents();

            this.PerformCommonResponseTests(bookmarksTorrents);
        }

        [TestMethod]
        [Description("Verifies GetBookmarksArtists method returns successfully and that response object contains at least one non-default property value")]
        public void GetBookmarksArtistsTest()
        {
            System.Threading.Thread.Sleep(2000);
            var bookmarksArtists = Api.GetBookmarksArtists();

            this.PerformCommonResponseTests(bookmarksArtists);
        }

        // TODO: GetArtist

        // TODO: GetTorrentGroup

        // TODO: DownloadTorrent

        // TODO: GetUser

        [TestMethod]
        [Description("Performs a random and broad user search then verifies a randomly selected range of pages validate successfully")]
        public void GetUserSearchTest()
        {
            // Search for a single letter
            string searchTerm = Helper.RandomCharString(1);
            System.Threading.Thread.Sleep(2000);
            var users = Api.GetUserSearch(searchTerm, 1);
            this.PerformCommonResponseTests(users);

            // Define a random page range
            int firstPage;
            int lastPage;
            if (users.response.pages > RandomRangeItems)
            {
                firstPage = Helper.GetRandomIntFromRange(1, users.response.pages - RandomRangeItems);
                lastPage = firstPage + RandomRangeItems;
            }
            else
            {
                firstPage = 1;
                lastPage = users.response.pages;
            }

            // Loop through page range
            for (int i = firstPage; i < lastPage; i++)
            {
                System.Threading.Thread.Sleep(2000);
                var userSearch = Api.GetUserSearch(searchTerm, i);

                this.PerformCommonResponseTests(userSearch);
                TestContext.WriteLine("Page {0} of {1} pass ok.", i, users.response.pages);
            }
        }

        // TODO: GetFlacLogScore

        private void PerformCommonResponseTests<T>(IResponse<T> deserializedJson, string additionalMsg = null)
        {
            Assert.IsNotNull(deserializedJson, "Response object is null" + (additionalMsg != null ? string.Format(" ({0})", additionalMsg) : ""));
            StringAssert.Matches(deserializedJson.status, new Regex("success", RegexOptions.IgnoreCase), string.Format("Unexpected response status ({0})", deserializedJson.status) + (additionalMsg != null ? string.Format(" ({0})", additionalMsg) : ""));
            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(deserializedJson.response), "JSON response data is null" + (additionalMsg != null ? string.Format(" ({0})", additionalMsg) : ""));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Api.Dispose();
        }

        private class SearchRequests : ISearchRequests
        {
            public string SearchTerm { get; set; }
            public int? Page { get; set; }
            public bool ShowFilled { get; set; }
            public string Tags { get; set; }
            public int? TagType { get; set; }
        }

        private class SearchInbox : IInbox
        {
            public string MessageType { get; set; }
            public bool DisplayUnreadFirst { get; set; }
            public int? Page { get; set; }
            public string SearchType { get; set; }
            public string SearchTerm { get; set; }
        }

    }
}