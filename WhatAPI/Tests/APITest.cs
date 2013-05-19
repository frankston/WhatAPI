using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatCD;
using WhatCD.ActionAnnouncements;

namespace Tests
{
    [TestClass]
    public class APITest
    {

        private static API Api;
        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Api = new API("YourUsername", "YourPassword");
        }


        [TestMethod]
        [Description("Verifies GetSubscriptions method returns successfully and that response object contains at least one non-default property value")]
        public void GetSubscriptionsTest()
        {
            System.Threading.Thread.Sleep(2000);
            var subscriptions = Api.GetSubscriptions(false);

            this.PerformCommonResponseTests(subscriptions);
        }


        [TestMethod]
        [Description("Verifies GetForumMain method returns successfully and that response object contains at least one non-default property value")]
        public void GetForumMain()
        {
            System.Threading.Thread.Sleep(2000);
            var forumMain = Api.GetForumMain();

            this.PerformCommonResponseTests(forumMain);
        }


        [TestMethod]
        [Description("Verifies GetForumViewForum method returns successfully and that response object contains at least one non-default property value")]
        public void GetForumViewForumTest()
        {
            int[] validForumIDs = new int[] { 7, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 23, 25, 26, 27, 29, 32, 36, 37, 40, 43, 45, 55, 56, 61, 62, 63, 65 };
            var forumID = (uint)Helper.GetRandomElementFromArray(validForumIDs);

            TestContext.WriteLine("Forum ID: " + forumID);

            uint currentPage = 0;
            uint totalPages;

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


        // TODO: GetForumViewThread


        // TODO: GetInbox


        // TODO: GetInboxViewConv


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


        // TODO: GetNotifications


        // TODO: GetTop10Torrents


        // TODO: GetTop10Tags


        // TODO: GetTop10Users


        // TODO: GetRequest


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


        // TODO: GetUserSearch


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

    }
}