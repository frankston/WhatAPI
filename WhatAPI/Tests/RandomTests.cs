using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhatCD;

namespace Tests
{
    [TestClass]
    public class RandomTests
    {

        [TestMethod]
        public void GetTorrentHashTest()
        {
            var hash = Credentials.WhatRandom.GetTorrentHash();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(hash), "Hash is null or white space");
        }

        [TestMethod]
        public void GetArtistNameTest()
        {
            var name = Credentials.WhatRandom.GetArtistName();
            Assert.IsTrue(!string.IsNullOrWhiteSpace(name), "Artist name is null or white space");
        }

        [TestMethod]
        public void GetArtistIdTest()
        {
            var id = Credentials.WhatRandom.GetArtistId();
            Assert.IsTrue(id > 0, string.Format("Artist id is zero or less than zero: '{0}'", id));
        }

        [TestMethod]
        public void GetIdOfArtistThatHasSimilarArtistsTest()
        {
            var id = Credentials.WhatRandom.GetIdOfArtistThatHasSimilarArtists();
            Assert.IsTrue(id > 0, string.Format("Artist id is zero or less than zero: '{0}'", id));
        }

        [TestMethod]
        public void GetIdOfArtistThatHasGroupsTest()
        {
            var id = Credentials.WhatRandom.GetIdOfArtistThatHasGroups();
            Assert.IsTrue(id > 0, string.Format("Artist id is zero or less than zero: '{0}'", id));
        }

        [TestMethod]
        public void GetTorrentIdTest()
        {
            var id = Credentials.WhatRandom.GetTorrentId();
            Assert.IsTrue(id > 0, string.Format("Torrent id is zero or less than zero: '{0}'", id));
        }

        [TestMethod]
        public void GetGroupIdTest()
        {
            var id = Credentials.WhatRandom.GetGroupId();
            Assert.IsTrue(id > 0, string.Format("Group id is zero or less than zero: '{0}'", id));
        }

        [TestMethod]
        public void GetUserIdTest()
        {
            var id = Credentials.WhatRandom.GetUserId();
            Assert.IsTrue(id > 0, string.Format("User id is zero or less than zero: '{0}'", id));
        }

        [TestMethod]
        public void GetForumIdTest()
        {
            var id = Credentials.WhatRandom.GetForumId(50);
            Assert.IsTrue(id > 0, string.Format("Forum id is zero or less than zero: '{0}'", id));
        }

        [TestMethod]
        public void GetForumThreadIdTest()
        {
            var id = Credentials.WhatRandom.GetForumThreadId(50);
            Assert.IsTrue(id > 0, string.Format("Forum thread id is zero or less than zero: '{0}'", id));
        }

    }
}
