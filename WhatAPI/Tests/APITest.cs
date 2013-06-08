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
    public class APITest
    {

        private static API Api;
        private const int MAX_RANDOM_RANGE_SIZE = 20;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Api = new API("YourUsername", "YourPassword") { ErrorOnMissingMember = true };
        }

        [TestMethod]
        [Description("Verifies GetSubscriptions method returns successfully and that response object contains at least one non-default property value")]
        public void GetSubscriptionsTest()
        {
            var subscriptions = Api.GetSubscriptions(true);
            this.PerformCommonResponseTests(subscriptions);
        }

        [TestMethod]
        [Description("Verifies GetForumMain method returns successfully and that response object contains at least one non-default property value")]
        public void GetForumMainTest()
        {
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
            // TODO: Make this loop a separate method
            do
            {
                currentPage++;
                var forum = Api.GetForumViewForum(forumID, currentPage);

                this.PerformCommonResponseTests(forum, string.Format("forum id: {0}, page: {1}", forumID, currentPage));

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
                currentPage++;
                var forum = Api.GetForumViewThread(forumID, currentPage);

                this.PerformCommonResponseTests(forum, string.Format("forum id: {0}, page: {1}", forumID, currentPage));

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
            var index = Api.GetIndex();
            this.PerformCommonResponseTests(index);
        }

        [TestMethod]
        [Description("Verifies GetStatus method returns a non-null object")]
        public void GetStatusTest()
        {
            var status = Api.GetStatus();
            Assert.IsNotNull(status);
        }

        [TestMethod]
        [Description("Verifies GetUptime method returns a non-null object")]
        public void GetUptimeTest()
        {
            var uptime = Api.GetUptime();
            Assert.IsNotNull(uptime);
        }

        [TestMethod]
        [Description("Verifies GetRecords method returns a non-null object")]
        public void GetRecordsTest()
        {
            var records = Api.GetRecords();
            Assert.IsNotNull(records);
        }

        [TestMethod]
        [Description("Verifies GetAnnouncements method returns successfully and that response object contains at least one non-default property value")]
        public void GetAnnouncementsTest()
        {
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
            var top10Torrents = Api.GetTop10Torrents(10);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        [Description("Verifies GetTop10Torrents method (with a limit of 25) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TorrentsTest_Limit25()
        {
            var top10Torrents = Api.GetTop10Torrents(25);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        [Description("Verifies GetTop10Torrents method (with a limit of 100) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TorrentsTest_Limit100()
        {
            var top10Torrents = Api.GetTop10Torrents(100);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        [Description("Verifies GetTop10Torrents method (with a limit of 250) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TorrentsTest_Limit250()
        {
            var top10Torrents = Api.GetTop10Torrents(250);
            this.PerformCommonResponseTests(top10Torrents);
        }

        [TestMethod]
        [Description("Verifies GetTop10Tags method (with a limit of 10) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TagsTest_Limit10()
        {
            var top10Tags = Api.GetTop10Tags(10);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        [Description("Verifies GetTop10Tags method (with a limit of 25) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TagsTest_Limit25()
        {
            var top10Tags = Api.GetTop10Tags(25);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        [Description("Verifies GetTop10Tags method (with a limit of 100) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TagsTest_Limit100()
        {
            var top10Tags = Api.GetTop10Tags(100);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        [Description("Verifies GetTop10Tags method (with a limit of 250) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10TagsTest_Limit250()
        {
            var top10Tags = Api.GetTop10Tags(250);
            this.PerformCommonResponseTests(top10Tags);
        }

        [TestMethod]
        [Description("Verifies GetTop10Users method (with a limit of 10) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10UsersTest_Limit10()
        {
            var top10Users = Api.GetTop10Users(10);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        [Description("Verifies GetTop10Users method (with a limit of 25) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10UsersTest_Limit25()
        {
            var top10Users = Api.GetTop10Users(25);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        [Description("Verifies GetTop10Users method (with a limit of 100) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10UsersTest_Limit100()
        {
            var top10Users = Api.GetTop10Users(100);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        [Description("Verifies GetTop10Users method (with a limit of 250) returns successfully and that response object contains at least one non-default property value")]
        public void GetTop10UsersTest_Limit250()
        {
            var top10Users = Api.GetTop10Users(250);
            this.PerformCommonResponseTests(top10Users);
        }

        [TestMethod]
        [Description("Performs a random broad search for torrents, selects a random page from the results and calls the GetTorrent method for each torrent (from a random range segment) and validates the response")]
        public void GetTorrentTest_ById()
        {
            var searchDetails = new SearchTorrents() { GroupName = Helper.RandomCharString(1) };
            var browse = Api.GetBrowse(searchDetails);
            this.PerformCommonResponseTests(browse);
            TestContext.WriteLine("Call to search for all torrents with name '{0}' complete", searchDetails.GroupName);

            // Select random page from results
            searchDetails.Page = Helper.GetRandomIntFromRange(1, browse.response.pages);
            var randomPagebrowse = Api.GetBrowse(searchDetails);

            // Select random range from the results on the random page
            var rangeDef = new RangeSelector(1, randomPagebrowse.response.results.Count, MAX_RANDOM_RANGE_SIZE);

            // TODO: Calulate total number of torrents that will be tested (for TestContext output)

            // Loop through random result range
            for (int i = rangeDef.RandomRangeStart; i < rangeDef.RandomRangeEnd; i++)
            {
                if (randomPagebrowse.response.results[i].torrents != null)
                {
                    // Loop through all torrents in each result
                    for (int j = 0; j < randomPagebrowse.response.results[i].torrents.Count; j++)
                    {
                        var getTorrent = Api.GetTorrent(randomPagebrowse.response.results[i].torrents[j].torrentId);
                        this.PerformCommonResponseTests(getTorrent);
                        TestContext.WriteLine("Torrent id {0} on result page {1} pass ok.", randomPagebrowse.response.results[i].torrents[j].torrentId, searchDetails.Page);
                    }
                }
            }
        }

        [TestMethod]
        [Description("Performs a random broad search for torrents, selects a random page from the results and calls the GetTorrent method for each torrent (from a random range segment) and validates the response")]
        public void GetTorrentTest_ByHash()
        {
            var searchDetails = new SearchTorrents() { GroupName = Helper.RandomCharString(1) };
            var browse = Api.GetBrowse(searchDetails);
            this.PerformCommonResponseTests(browse);
            TestContext.WriteLine("Call to search for all torrents with name '{0}' complete", searchDetails.GroupName);

            // Select random page from results
            searchDetails.Page = Helper.GetRandomIntFromRange(1, browse.response.pages);
            var randomPagebrowse = Api.GetBrowse(searchDetails);

            // Select random range from the results on the random page
            var rangeDef = new RangeSelector(1, randomPagebrowse.response.results.Count, MAX_RANDOM_RANGE_SIZE);

            // Loop through random result range
            for (int i = rangeDef.RandomRangeStart; i < rangeDef.RandomRangeEnd; i++)
            {
                if (randomPagebrowse.response.results[i].torrents != null)
                {
                    // Loop through all torrents in each result
                    for (int j = 0; j < randomPagebrowse.response.results[i].torrents.Count; j++)
                    {
                        // First we must get the torrent by ID as the result will have the torrents hash
                        var torrentById = Api.GetTorrent(randomPagebrowse.response.results[i].torrents[j].torrentId);
                        this.PerformCommonResponseTests(torrentById);
                        TestContext.WriteLine("Get torrent (by id) {0} on result page {1} pass ok.", randomPagebrowse.response.results[i].torrents[j].torrentId, searchDetails.Page);

                        var torrentByHash = Api.GetTorrent(torrentById.response.torrent.infoHash);
                        this.PerformCommonResponseTests(torrentById);
                        TestContext.WriteLine("Get torrent (by hash) {0} on result page {1} pass ok.", torrentById.response.torrent.infoHash, searchDetails.Page);

                        Assert.IsTrue(Helper.AreEqual<string>(torrentById.response.torrent.description, torrentByHash.response.torrent.description), "Torrent descriptions are different between objects returned by GetTorrent by ID and GetTorrent by hash");
                    }
                }
            }
        }

        [TestMethod]
        [Description("Verifies that a randomly selected range of request pages validate successfully")]
        public void GetRequestsTest()
        {
            // Get first pages of requests
            SearchRequests searchArgs = new SearchRequests() { ShowFilled = true };
            var requests = Api.GetRequests(searchArgs);
            this.PerformCommonResponseTests(requests);

            // Define a random page range
            var rangeDef = new RangeSelector(1, requests.response.pages, MAX_RANDOM_RANGE_SIZE);

            // Loop through page range
            for (int i = rangeDef.RandomRangeStart; i < rangeDef.RandomRangeEnd; i++)
            {
                searchArgs.Page = i;
                var requestsPage = Api.GetRequests(searchArgs);
                this.PerformCommonResponseTests(requestsPage);
                TestContext.WriteLine("Page {0} of {1} pass ok (testing {2} pages in total).", i, requests.response.pages, rangeDef.ActualSegmentSize);
             }
        }

        [TestMethod]
        [Description("Verifies GetSimilarArtists method returns artists for random torrents returned by GetRequests")]
        public void GetSimilarArtistsTest()
        {

            // Get first pages of requests
            SearchRequests searchArgs = new SearchRequests() { ShowFilled = true };
            var requests = Api.GetRequests(searchArgs);
            this.PerformCommonResponseTests(requests);

            // Pick a random page from the pages
            var pageRange = Helper.GetRandomIntFromRange(1, requests.response.pages);

            // Build distinct artist ID list
            var artists = requests.response.results.Select(x => x.artists.Count > 0 ? x.artists[0][0].id : (int?)null).Where(c => c != null && c > 0).Distinct().ToList();

            // Iterate through all artists on the page
            for (int i = 0; i < artists.Count(); i++)
            {
                var similarArtists = Api.GetSimilarArtists((int)artists[i], 100);
                if (similarArtists.artists != null) // Some artists have no similar artists
                {
                    for (int j = 0; j < similarArtists.artists.Count; j++)
                    {
                        if (similarArtists.artists.Count > 0)
                        {
                            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(similarArtists.artists[j]), "Similar artist ({0}) info is null", similarArtists.artists[j]);
                        }
                    }
                }

                TestContext.WriteLine("Artist '{0}' ({1} of {2}) pass ok.", artists[i], i, artists.Count() - 1);
            }
        }

        [TestMethod]
        [Description("Performs a random broad search for torrents, selects a random range of pages from the results and loads and validates each page within the range")]
        public void GetBrowseTest()
        {
            var searchDetails = new SearchTorrents() { GroupName = Helper.RandomCharString(1) };
            var browse = Api.GetBrowse(searchDetails);
            this.PerformCommonResponseTests(browse);

            // Define a random range of pages to parse
            var rangeDef = new RangeSelector(1, browse.response.pages, MAX_RANDOM_RANGE_SIZE);

            // Iterate through random page range
            for (int i = rangeDef.RandomRangeStart; i < rangeDef.RandomRangeEnd; i++)
            {
                searchDetails.Page = i;
                var getBrowsePage = Api.GetBrowse(searchDetails);
                this.PerformCommonResponseTests(getBrowsePage);
                TestContext.WriteLine("Page {0} of {1} pass ok (testing {2} pages in total).", i, getBrowsePage.response.pages, rangeDef.ActualSegmentSize);
            }
        }

        [TestMethod]
        [Description("Verifies GetBookmarksTorrents method returns successfully and that response object contains at least one non-default property value")]
        public void GetBookmarksTorrentsTest()
        {
            var bookmarksTorrents = Api.GetBookmarksTorrents();
            this.PerformCommonResponseTests(bookmarksTorrents);
        }

        [TestMethod]
        [Description("Verifies GetBookmarksArtists method returns successfully and that response object contains at least one non-default property value")]
        public void GetBookmarksArtistsTest()
        {
            var bookmarksArtists = Api.GetBookmarksArtists();
            this.PerformCommonResponseTests(bookmarksArtists);
        }

        [TestMethod]
        [Description("Verifies GetArtist method returns an artist based on random torrents returned by a GetRequests search")]
        public void GetArtistTest_ByName()
        {
            // TODO: This code should work but does not. Not all artists returned from GetBrowse are valid WhatCD artists...

            //var searchDetails = new SearchTorrents() {  ArtistName = Helper.RandomCharString(1) };
            //var browse = Api.GetBrowse(searchDetails);
            //this.PerformCommonResponseTests(browse);

            //// Pick a random page from the pages
            //var pageRange = Helper.GetRandomIntFromRange(1, browse.response.pages);

            //// Build distinct artist list
            //var artists = browse.response.results.Select(x => HttpUtility.HtmlDecode(x.artist)).Distinct().ToList();

            //// Iterate through all artists on the page
            //for (int i = 0; i < artists.Count(); i++)
            //{
            //    var artist = Api.GetArtist(artists[i]);
            //    this.PerformCommonResponseTests(artist, artists[i]);
            //    TestContext.WriteLine("Artist '{0}' ({1} of {2}) pass ok.", artists[i], i, browse.response.results.Count());
            //}

            // Get first pages of requests
            SearchRequests searchArgs = new SearchRequests() { ShowFilled = true };
            var requests = Api.GetRequests(searchArgs);
            this.PerformCommonResponseTests(requests);

            // Pick a random page from the pages
            var pageRange = Helper.GetRandomIntFromRange(1, requests.response.pages);

            // Build distinct artist list
            var artists = requests.response.results.Select(x => x.artists.Count > 0 ? HttpUtility.HtmlDecode(x.artists[0][0].name) : "").Where(c => c != null).Distinct().ToList();

            // Iterate through all artists on the page
            for (int i = 0; i < artists.Count(); i++)
            {
                var artist = Api.GetArtist(artists[i]);
                this.PerformCommonResponseTests(artist, artists[i]);
                TestContext.WriteLine("Artist '{0}' ({1} of {2}) pass ok.", artists[i], i, artists.Count() - 1);
            }
        }

        [TestMethod]
        [Description("Verifies GetArtist method returns an artist based on random torrents returned by a GetRequests search")]
        public void GetArtistTest_ById()
        {

            // Get first pages of requests
            SearchRequests searchArgs = new SearchRequests() { ShowFilled = true };
            var requests = Api.GetRequests(searchArgs);
            this.PerformCommonResponseTests(requests);

            // Pick a random page from the pages
            var pageRange = Helper.GetRandomIntFromRange(1, requests.response.pages);

            // Build distinct artist list
            var artists = requests.response.results.Select(x => x.artists.Count > 0 ? x.artists[0][0].id : (int?)null).Where(c => c != null).Distinct().ToList();

            // Iterate through all artists on the page
            for (int i = 0; i < artists.Count(); i++)
            {
                var artist = Api.GetArtist((int)artists[i]);
                this.PerformCommonResponseTests(artist, artists[i].ToString());
                TestContext.WriteLine("Artist '{0}' ({1} of {2}) pass ok.", artists[i], i, artists.Count() - 1);
            }
        }

        // TODO: GetTorrentGroup

        // TODO: DownloadTorrent

        // TODO: GetUser

        [TestMethod]
        [Description("Performs a random and broad user-search then verifies a randomly selected range of pages validate successfully")]
        public void GetUserSearchTest()
        {
            // Search for a single letter
            string searchTerm = Helper.RandomCharString(1);
            var users = Api.GetUserSearch(searchTerm, null);
            this.PerformCommonResponseTests(users, searchTerm);

            // Define a random range of pages to parse
            var rangeDef = new RangeSelector(1, users.response.pages, MAX_RANDOM_RANGE_SIZE);

            // Iterate through random page range
            for (int i = rangeDef.RandomRangeStart; i < rangeDef.RandomRangeEnd; i++)
            {
                var userSearch = Api.GetUserSearch(searchTerm, i);

                this.PerformCommonResponseTests(userSearch);
                TestContext.WriteLine("Page {0} of {1} pass ok (testing {2} pages in total).", i, users.response.pages, rangeDef.ActualSegmentSize);
            }
        }

        // TODO: GetFlacLogScore

        private void PerformCommonResponseTests<T>(IResponse<T> deserializedJson, string argsInfo = null)
        {
            argsInfo = argsInfo != null ? string.Format(" (args info: {0})", argsInfo) : null;
            Assert.IsNotNull(deserializedJson, "Response object is null" + (argsInfo != null ? argsInfo : ""));
            StringAssert.Matches(deserializedJson.status, new Regex("success", RegexOptions.IgnoreCase), string.Format("Unexpected response status ({0})", deserializedJson.status) + (argsInfo != null ? argsInfo : ""));
            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(deserializedJson.response), "JSON response data is null" + (argsInfo != null ? argsInfo : ""));
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
            public bool? ShowFilled { get; set; }
            public string Tags { get; set; }
            public int? TagsType { get; set; }
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