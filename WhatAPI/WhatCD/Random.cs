using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using WhatCD.Model;
using WhatCD.Model.ActionForum.TypeViewThread;
using WhatCD.Model.ActionForum.TypeViewForum;
using WhatCD.Model.ActionRequests;
using System.Web;

namespace WhatCD
{
    public class Random
    {
        public Api Api { get; private set; }

        private static System.Random random = new System.Random((int)DateTime.Now.Ticks);

        public Random(Api api)
        {
            this.Api = api;
        }

        public string GetTorrentHash()
        {
            var torrent = Api.GetTorrent(this.GetTorrentId());
            return torrent.response.torrent.infoHash;
        }

        public Requests GetRequests()
        {
            SearchRequests searchArgs = new SearchRequests() { ShowFilled = true };
            var requests = Api.GetRequests(searchArgs);
            var page = GetRandomIntFromRange(1, requests.response.pages - 1);
            searchArgs.Page = page;
            return Api.GetRequests(searchArgs);
        }

        public string GetArtistName()
        {
            // Using requests because not all artists returned from GetBrowse are valid WhatCD artists...
            var requests = this.GetRequests();

            // Build distinct artist name list
            var artists = requests.response.results
                .Where(x => x.artists != null && x.artists.Count > 0)
                .Select(x => x.artists[0][0].name).Distinct().ToList();

            if (artists.Count > 0)
            {
                return artists[GetRandomIntFromRange(0, artists.Count - 1)];
            }
            else
            {
                throw new ArgumentNullException("No similar artist ids found on requests page");
            }
        }

        public int GetArtistId()
        {
            // Using requests because not all artists returned from GetBrowse are valid WhatCD artists...
            var requests = this.GetRequests();

            // Build distinct artist id list
            var artists = requests.response.results
                .Where(x => x.artists != null && x.artists.Count > 0)
                .Select(x => x.artists[0][0].id).Distinct().ToList();

            if (artists.Count > 0)
            {
                return artists[GetRandomIntFromRange(0, artists.Count - 1)];
            }
            else
            {
                throw new ArgumentNullException("No similar artist ids found on requests page");
            }
        }

        public int GetIdOfArtistThatHasSimilarArtists()
        {
            var requests = this.GetRequests();

            // Build distinct artist id list
            var artists = requests.response.results
                .Where(x => x.artists != null && x.artists.Count > 0)
                .Select(x => x.artists[0][0].id).Distinct().ToList();

            if (artists.Count > 0)
            {
                // Iterate through all artists on the page
                for (int i = 0; i < artists.Count(); i++)
                {
                    var similarArtists = Api.GetSimilarArtists(artists[i], 100);
                    if (similarArtists.artists != null && similarArtists.artists.Count > 0)
                    {
                        return (int)artists[i]; // Some artists have no similar artists...
                    }
                }
            }
            throw new ArgumentNullException(string.Format("No similar artists found on requests page {0}", requests.response.currentPage));
        }

        public int GetIdOfArtistThatHasGroups()
        {
            var requests = this.GetRequests();

            // Build distinct artist id list
            var artists = requests.response.results
                .Where(x => x.artists != null && x.artists.Count > 0)
                .Select(x => x.artists[0][0].id).Distinct().ToList();

            if (artists.Count > 0)
            {
                // Iterate through all artists found on the page
                for (int i = 0; i < artists.Count(); i++)
                {
                    // Get artist
                    var artist = Api.GetArtist((int)artists[i]);

                    if (artist.response.torrentGroup != null && artist.response.torrentGroup.Count > 0)
                    {
                        return (int)artists[i];
                    }
                }
            }
            throw new ArgumentNullException(string.Format("No artists with groups found on requests page {0}", requests.response.currentPage));
        }

        public int GetTorrentId()
        {
            var searchDetails = new SearchTorrents() { GroupName = RandomCharString(1) };
            var browse = Api.GetBrowse(searchDetails);

            // Loop through random result range and return first populated torrent found
            for (int i = 1; i < browse.response.results.Count; i++)
            {
                if (browse.response.results[i].torrents != null && browse.response.results[i].torrents.Count > 0)
                {
                    var number = browse.response.results[i].torrents.Count == 1 ? 0 : GetRandomIntFromRange(0, browse.response.results[i].torrents.Count - 1);
                    return browse.response.results[i].torrents[number].torrentId;
                }
            }
            throw new ArgumentNullException(string.Format("No torrents found on first page of torrent search '{0}' results.", searchDetails.GroupName));
        }

        public int GetGroupId()
        {
            var artistId = this.GetIdOfArtistThatHasGroups();
            var artist = Api.GetArtist(artistId);

            if (artist.response.torrentGroup.Count > 0)
            {
                var number = GetRandomIntFromRange(0, artist.response.torrentGroup.Count);
                return artist.response.torrentGroup[number].groupId;
            }
            throw new ArgumentNullException(string.Format("No torrent groups found for artist id {0}.", artistId));
        }

        public int GetUserId()
        {
            // Select first page
            string searchTerm = RandomCharString(1);
            var searchPage1 = Api.GetUserSearch(searchTerm, null);

            // Select random page
            int page = 0;
            if (searchPage1.response.pages > 1)
            {
                page = GetRandomIntFromRange(2, searchPage1.response.pages);
                var searchPageX = Api.GetUserSearch(searchTerm, page);
                if (searchPageX.response.results.Count > 0)
                {
                    var number = searchPageX.response.results.Count == 1 ? 0 : GetRandomIntFromRange(0, searchPageX.response.results.Count - 1);
                    return searchPageX.response.results[number].userId;
                }
            }
            throw new ArgumentNullException(string.Format("No user ids found on page {0} of user search '{0}' results.", page, searchTerm));
        }

        public int GetForumId(int maxAttempts = 20)
        {
            // On my account the valid forum IDs are: 7, 8, 9, 10, 12, 13, 14, 15, 17, 18, 19, 20, 23, 25, 26, 27, 29, 32, 36, 37, 40, 43, 45, 55, 56, 61, 62, 63, 65
            Func<int, ForumViewForum> apiCall = (id) => Api.GetForumViewForum(id, 1);
            return BruteForceFindValidId<ForumViewForum, WhatCD.Model.ActionForum.TypeViewForum.Response>(apiCall, 5, 70, maxAttempts);
        }

        public int GetForumThreadId(int maxAttempts = 20)
        {
            Func<int, ForumViewThread> apiCall = (id) => Api.GetForumViewThread(id);
            return BruteForceFindValidId<ForumViewThread, WhatCD.Model.ActionForum.TypeViewThread.Response>(apiCall, 2000, 65000, maxAttempts);
        }

        private int BruteForceFindValidId<T, R>(Func<int, T> apiCall, int minRange, int maxRange, int maxAttempts)
        {
            var idsAlreadyTried = new List<int?>();
            
            while (maxAttempts > idsAlreadyTried.Count)
            {
                int currentId = 0;
                do 
                {
                    currentId = GetRandomIntFromRange(minRange, maxRange);
                } while (idsAlreadyTried.FirstOrDefault(x => x == currentId) != null);

                try
                {
                    Debug.WriteLine("Validating ID: " + currentId);
                    var apiCallResponse = (IResponse<R>)apiCall(currentId);
                    if (apiCallResponse == null)
                    {
                        idsAlreadyTried.Add(currentId);
                    }
                    else
                    {
                        if (!Regex.IsMatch(apiCallResponse.status, "success", RegexOptions.IgnoreCase))
                        {
                            idsAlreadyTried.Add(currentId);
                        }
                        else
                        {
                            return currentId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    idsAlreadyTried.Add(currentId);
                }
            }
            throw new OperationCanceledException(string.Format("The maximum number of attempts ({0}) to locate a valid ID has been reached", maxAttempts));
        }

        public static int GetRandomIntFromRange(int smallest, int largest, params int[] exclude)
        {
            if (smallest > largest) throw new ArgumentOutOfRangeException("Smallest is greater than largest");
            if (smallest == largest) throw new ArgumentOutOfRangeException("Smallest cannot be same as largest");
            while (true)
            {
                var value = random.Next(smallest, largest + 1);
                if (!exclude.Contains(value)) return value;
            }
        }

        public static string RandomCharString(int length)
        {
            char[] c = new char[length];
            for (int i = 0; i < length; i++) c[i] = (char)random.Next(97, 122);
            return new string(c);
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

        private class SearchRequests : ISearchRequests
        {
            public string SearchTerm { get; set; }
            public int? Page { get; set; }
            public bool? ShowFilled { get; set; }
            public string Tags { get; set; }
            public int? TagsType { get; set; }
        }
    }
}
