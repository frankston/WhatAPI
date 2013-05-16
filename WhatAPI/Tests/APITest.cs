using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatCD;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
            Api = new API("YourOwnUsername", "YourOwnPassword");
        }

        [TestMethod]
        [Description("Verifies GetIndex method returns successfully and that response object contains at least one non-default property value")]
        public void GetIndexTest()
        {
            System.Threading.Thread.Sleep(2000);
            var index = Api.GetIndex();

            Assert.IsNotNull(index, "Returned index object is null");
            StringAssert.Matches(index.status, new Regex("success", RegexOptions.IgnoreCase), "Unexpected JSON response status ({0})", index.status);
            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(index.response), "JSON response data is null");
        }

        [TestMethod]
        [Description("Verifies GetSubscriptions method returns successfully and that response object contains at least one non-default property value")]
        public void GetSubscriptionsTest()
        {
            System.Threading.Thread.Sleep(2000);
            var subscriptions = Api.GetSubscriptions(false);

            Assert.IsNotNull(subscriptions, "Returned subscriptions object is null");
            StringAssert.Matches(subscriptions.status, new Regex("success", RegexOptions.IgnoreCase), "Unexpected JSON response status ({0})", subscriptions.status);
            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(subscriptions.response), "JSON response data is null");
        }

        [TestMethod]
        [Description("Verifies GetForumCategories method returns successfully and that response object contains at least one non-default property value")]
        public void GetForumCategoriesTest()
        {
            System.Threading.Thread.Sleep(2000);
            var forumCategories = Api.GetForumCategories();

            Assert.IsNotNull(forumCategories, "Returned object is null");
            StringAssert.Matches(forumCategories.status, new Regex("success", RegexOptions.IgnoreCase), "Unexpected JSON response status ({0})", forumCategories.status);
            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(forumCategories.response), "JSON response data is null");
        }

        [TestMethod]
        [Description("Verifies GetForum method returns successfully and that response object contains at least one non-default property value")]
        public void GetForumTest()
        {
            // TODO: Make this test walk all the pages available within the selected forum
            System.Threading.Thread.Sleep(2000);

            int[] validForumIDs = new int[] { 7, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 23, 25, 26, 27, 29, 32, 36, 37, 40, 43, 45, 55, 56, 61, 62, 63, 65 };
            var forumID = (uint)Helper.GetRandomElementFromArray(validForumIDs);

            TestContext.WriteLine("Forum ID: " + forumID);

            var forum = Api.GetForum(forumID, 1);
            Assert.IsNotNull(forum, "Returned object is null (forum ID {0})", forumID);
            StringAssert.Matches(forum.status, new Regex("success", RegexOptions.IgnoreCase), "Unexpected JSON response status ({0}) (forum ID {1})", forum.status, forumID);
            Assert.IsFalse(Helper.AllPropertiesAreDefaultValues(forum.response), "JSON response data is null (forum ID {1})", forumID);
        }

        // TODO: Create at least one test method for each method in the API class

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Api.Dispose();
        }
    }
}