using System;
using System.Collections.Generic;
using System.Linq;
using TweetSharp;
using System.IO;
using System.Diagnostics;

namespace WCCFNew
{
    class Twit//MASTER TWIT
    {
        private TwitterService twitter;
        private static TwitterClientInfo info = new TwitterClientInfo() { ConsumerKey = "72GEFnweNQ9x9k9XJzqRUUjhn", ConsumerSecret = "gGEaHCA5PN4TxDf91qNAaqQT7SjFczLRq299KvrnuaatD23ihT" };
        private TwitterUser user;
        private OAuthRequestToken req;
        private OAuthAccessToken authToken;



        public Twit(string ConsumerKey, string ConsumerSecret, string AToken, string ATokenSecret, long aUserId, string aScreenName)
        {
            twitter = new TwitterService(ConsumerKey, ConsumerSecret);
            twitter.AuthenticateWith(AToken, ATokenSecret);
            req = twitter.GetRequestToken();
            authToken = new OAuthAccessToken() { Token = AToken, TokenSecret = ATokenSecret, UserId = aUserId, ScreenName = aScreenName };
            twitter.GetUserProfile(new GetUserProfileOptions { });
        }

        public Twit(string AToken, string ATokenSecret, long AUserId, string AscreenName) : this(info.ConsumerKey, info.ConsumerSecret, AToken, ATokenSecret, AUserId, AscreenName) { }


        /// <summary>
        /// constuctor that authorizes
        /// should go in the order of constuctor, authorize
        /// </summary>
        public Twit()
        {
            twitter = new TwitterService(info.ConsumerKey, info.ConsumerSecret);
            req = twitter.GetRequestToken();
            string u = twitter.GetAuthenticationUrl(req).ToString();
            Process.Start(u);

        }
        /// authorizes the  user
        /// </summary>
        /// <param name="verify">verification code</param>
        /// <returns>true if successful</returns>
        public bool authorize(string verify)
        {
            authToken = twitter.GetAccessToken(req, verify);
            twitter.AuthenticateWith(info.ConsumerKey, info.ConsumerSecret, authToken.Token, authToken.TokenSecret);
            user = twitter.GetUserProfile(new GetUserProfileOptions { });
            return twitter.Response.Error == null;
        }
        public string getAuthTokenAsString()
        { return authToken.Token; }
        public string getAuthSecretAsString()
        { return authToken.TokenSecret; }


        /// <summary>
        /// simple post to the twitter account linked to the CKey and Token
        /// </summary>
        /// <param name="text">what the post will comprise of</param>
        /// <returns>true if it is successful</returns>
        public bool post(string text)
        {
            int beforetc = getUserTweetCount();
            TwitterStatus status = twitter.SendTweet(new SendTweetOptions() { Status = text });
            return getUserTweetCount(getUserHandle()) - beforetc >= 1;
            //return true;//temp
        }

        /// <summary>
        /// posts media with an image attached
        /// </summary>
        /// <param name="text">text to be added to the tweet</param>
        /// <param name="imagePath">path of the image being tweeted</param>
        /// <returns>true if the tweet was successful</returns>
        public bool post(string text, string imagePath)
        {
            if (text.Length > 117)
            { text = text.Substring(0, 117); }
            int beforetc = getUserTweetCount();
            Dictionary<string, Stream> dict = new Dictionary<string, Stream>();
            FileStream stream = new FileStream(imagePath, FileMode.Open);
            dict.Add(imagePath, stream);
            TwitterStatus status = twitter.SendTweetWithMedia(new SendTweetWithMediaOptions() { Status = text, Images = dict });
            return getUserTweetCount(getUserHandle()) - beforetc >= 1;
        }

        /// <summary>
        /// posts media with an image attached
        /// </summary>
        /// <param name="text">text to be added to the tweet</param>
        /// <param name="imagePaths">array containing the paths of the images being tweeted  if length>4, first 4 are chosen</param>
        /// <returns>true if the tweet was successful</returns>
        public bool post(string text, string[] imagePaths)
        {
            if (imagePaths.Length <= 0) return post(text);
            int beforetc = getUserTweetCount();
            Dictionary<string, Stream> dict = new Dictionary<string, Stream>();
            int checker = 0;
            foreach (string imagePath in imagePaths)
            {
                if (checker > 4) break;
                checker++;
                FileStream stream = new FileStream(imagePath, FileMode.Open);
                dict.Add(imagePath, stream);
            }
            TwitterStatus status = twitter.SendTweetWithMedia(new SendTweetWithMediaOptions() { Status = text, Images = dict });
            return getUserTweetCount(getUserHandle()) - beforetc >= 1;
        }

        /// <summary>
        /// posts media with an image attached
        /// </summary>
        /// <param name="text">text to be tweeted</param>
        /// <param name="images">dictionary of images   if count>4, exception thrown  string = image path, stream = filestream(image path, fileMode.Open)</param>
        /// <returns>true if the post was successful</returns>
        public bool post(string text, Dictionary<string, Stream> images)
        {
            if (images.Count > 4) throw new Exception("dictionary cannot have >4 picture paths");
            int beforetc = getUserTweetCount();
            TwitterStatus status = twitter.SendTweetWithMedia(new SendTweetWithMediaOptions() { Status = text, Images = images });
            return getUserTweetCount(getUserHandle()) - beforetc >= 1;
        }


        /// <summary>
        /// gives the name of the user linked to the CKey and Token
        /// </summary>
        /// <returns>name of the user </returns>
        public string getUserName()
        { return getUserName(authToken.ScreenName); }

        /// <summary>
        /// gives the name of the target
        /// </summary>
        /// <param name="targetName">handle of target</param>
        /// <returns>name of the target based off of the handle</returns>
        public string getUserName(string targetName)
        { return searchUser(targetName).Name; }

        /// <summary>
        /// returns the twitter handle of the user linked to the CKey and Token
        /// </summary>
        /// <returns>user twitter handle</returns>
        public string getUserHandle()
        { return authToken.ScreenName; }

        /// <summary>
        /// returns the twitter handle of the targetName
        /// THIS METHOD IS FOR SPECIAL CASES; TARGETNAME==RETURN VALUE
        /// </summary>
        /// <param name="targetName">twitter handle of target</param>
        /// <returns>twitter handle of target</returns>
        public string getUserHandle(string targetName)
        { return searchUser(targetName).ScreenName; }

        /// <summary>
        /// gives the ID of the user linked to the CKey and Token
        /// </summary>
        /// <returns>ID of the user</returns>
        public Int64 getUserID()
        { return getUserID(authToken.ScreenName); }

        /// <summary>
        /// gets the user Id of the target
        /// </summary>
        /// <param name="targetName">handle of the target</param>
        /// <returns>id of the target</returns>
        public Int64 getUserID(string targetName)
        { return searchUser(targetName).Id; }

        /// <summary>
        /// returns the number of tweets favorited by the account linked to the CKey and Token
        /// </summary>
        /// <returns>number of tweets favorited by the account</returns>
        public int getUserFavoritesCount()
        { return getFavorites(authToken.ScreenName).Count(); }

        /// <summary>
        /// returns the number of tweets favorited by the target
        /// </summary>
        /// <param name="targetName">handle of the target</param>
        /// <returns>number of tweets favorited by the target</returns>
        public int getUserFavoritesCount(string targetName)
        { return getFavorites(targetName).Count(); }

        /// <summary>
        /// returns the number of followers of the account linked to the CKey and Token
        /// </summary>
        /// <returns>number of followers</returns>
        public int getUserFollowerCount()
        { return getFollowers(authToken.ScreenName).Count(); }

        /// <summary>
        /// returns the number of followers of the target
        /// </summary>
        /// <param name="targetName">handle of the target</param>
        /// <returns>the number of followers the target has</returns>
        public int getUserFollowerCount(string targetName)
        { return getFollowers(targetName).Count(); }

        /// <summary>
        /// gets the number of tweets made by the user linked to the CKey and Token
        /// </summary>
        /// <returns>number of tweets</returns>
        public int getUserTweetCount()
        { return getTweets(authToken.ScreenName).Count(); }

        /// <summary>
        /// gets the number of Tweets made by the target
        /// </summary>
        /// <param name="targetName">handle of the target</param>
        /// <returns>number of tweets by the target</returns>
        public int getUserTweetCount(string targetName)
        { return getTweets(targetName).Count(); }

        /// <summary>
        /// sends a direct message to the target
        /// </summary>
        /// <param name="targetName">twitter handle of target</param>
        /// <param name="text">message to be sent</param>
        public void directMessage(string targetName, string text)
        { twitter.SendDirectMessage(new SendDirectMessageOptions() { ScreenName = targetName, Text = text }); }

        /// <summary>
        /// follows specified user
        /// </summary>
        /// <param name="targetName">user to be followed</param>
        public void followUser(string targetName)
        { twitter.FollowUser(new FollowUserOptions() { ScreenName = targetName }); }

        /// <summary>
        /// unfollows specified user
        /// </summary>
        /// <param name="targetName">user to be unfollowed</param>
        public void unfollowUser(string targetName)
        { twitter.UnfollowUser(new UnfollowUserOptions() { ScreenName = targetName }); }

        /// <summary>
        /// gives the names of the followers of the user linked to the AToken
        /// </summary>
        /// <returns>array of strings containing user names</returns>
        public string[] getFollowerNames()
        { return getFollowerNames(authToken.ScreenName); }

        /// <summary>
        /// gives the names of the followers of the target
        /// </summary>
        /// <returns>array of strings containing user names</returns>
        /// <param name="targetName">user to grab followers from</param>
        public string[] getFollowerNames(string targetName)
        {
            List<string> res = new List<string>();
            foreach (var item in getFollowers(targetName))
            { res.Add(item.Name); }
            return res.ToArray();
        }

        /// <summary>
        /// returns the tweets of the user  linked to the CKey and Token as strings
        /// </summary>
        /// <returns>array of strings containing the tweet text<returns>
        public string[] getTweetsAsArray()
        { return getTweetsAsArray(authToken.ScreenName); }

        /// <summary>
        /// returns the tweets of the target
        /// </summary>
        /// <param name="targetName">name of user to pull tweets from</param>
        /// <returns>array of strings containing the tweet text<returns>
        public string[] getTweetsAsArray(string targetName)
        {
            List<string> res = new List<string>();
            foreach (var item in getTweets(targetName))
            { res.Add(item.Text); }
            return res.ToArray();
        }

        /// <summary>
        /// grabs tweets from the user connected with the access token
        /// </summary>
        /// <returns>list of the user's tweets</returns>
        private IEnumerable<TwitterStatus> getTweets()
        { return getTweets(authToken.ScreenName); }

        /// <summary>
        /// list of tweets for the target
        /// </summary>
        /// <param name="targetName">target whose tweets are being grabbed</param>
        /// <returns>list of the target's tweets</returns>
        private IEnumerable<TwitterStatus> getTweets(string targetName)
        { return twitter.ListTweetsOnSpecifiedUserTimeline(screenName: targetName ); }
        //{ return twitter.ListTweetsOnUserTimeline(new ListTweetsOnUserTimelineOptions() { ScreenName = targetName }); }

        /// <summary>
        /// grabs all the tweets the user linked to the access token favorited
        /// </summary>
        /// <returns>list of favorited tweets</returns>
        private IEnumerable<TwitterStatus> getFavorites()
        { return getFavorites(authToken.ScreenName); }

        /// <summary>
        /// grabs all the tweets favorited by the target
        /// </summary>
        /// <param name="targetName">handle of the target</param>
        /// <returns>list of tweets that were favorited by the target</returns>
        private IEnumerable<TwitterStatus> getFavorites(string targetName)
        { return twitter.ListFavoriteTweets(new ListFavoriteTweetsOptions() { ScreenName = targetName }); }

        /// <summary>
        /// gets list of the followers of the user linked to the acces token
        /// </summary>
        /// <returns>list of the user's followers</returns>
        private IEnumerable<TwitterUser> getFollowers()
        { return getFollowers(authToken.ScreenName); }

        /// <summary>
        /// gets list of the followers of the target
        /// </summary>
        /// <param name="targetName">handle of the target</param>
        /// <returns>list of the target's followers</returns>
        private IEnumerable<TwitterUser> getFollowers(string targetName)
        { return twitter.ListFollowers(new ListFollowersOptions() { ScreenName = targetName }); }

        /// <summary>
        /// returns the first user in the list returned when searching
        /// </summary>
        /// <param name="name">name to be searched</param>
        /// <returns>a TwitterUser class of the first user in search</returns>
        private TwitterUser searchUser(string name)
        { return twitter.SearchForUser(new SearchForUserOptions() { Q = name }).ElementAt(0); }

        /// <summary>
        /// returns the user in the list at the specified index
        /// </summary>
        /// <param name="name">name to be searched</param>
        /// <param name="index">index user is in the list</param>
        /// <returns>the user in the list at a specified index</returns>
        private TwitterUser searchUser(string name, int index)
        { return twitter.SearchForUser(new SearchForUserOptions() { Q = name }).ElementAt(0); }

    }
}
