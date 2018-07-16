using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTwitterApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["consumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"],
                    AccessToken = ConfigurationManager.AppSettings["accessToken"],
                    AccessTokenSecret = ConfigurationManager.AppSettings["accessTokenSecret"]
                }
            };

            var twitterCtx = new TwitterContext(auth);
            //FollowerIds("OtherHoe",twitterCtx);
            //FollowerScreenName("OtherHoe", twitterCtx);
            //Tweet("Pizza for breakfast and ??.", twitterCtx);
            UserTimeLine("OtherHoe", twitterCtx);





            //followers.IDInfo.IDs.ForEach(id =>Console.WriteLine("Follower ID: " + id));

            //Console.WriteLine(followers.IDInfo.IDs.Count());

        }

        public static void Tweet(string message, TwitterContext twitterCtx)
        {
            var status =
                message+" \n " +
                /*DateTime.Now.ToString() +*/ "";

            Console.WriteLine("\nStatus being sent: \n\n\"{0}\"", status);
            Console.WriteLine("\nPress any key to post tweet...\n");
            Console.ReadKey();

            var tweet = twitterCtx.TweetAsync(status).Result;

            Console.WriteLine(
                        "Status returned: " +
                        "(" + tweet.StatusID + ")" +
                        tweet.User.Name + ", " +
                        tweet.Text + "\n");
        }


        public static void FollowerScreenName(String screenName,TwitterContext twitterCtx)
        {
            var followers =
               (from follower in twitterCtx.Friendship
                where follower.Type == FriendshipType.FriendsList &&
                      follower.ScreenName == screenName
                select follower)
                .SingleOrDefault();

            followers.Users.ForEach(friend =>
                    Console.WriteLine(
                        "ID: {0} Name: {1}",
                        friend.UserIDResponse, friend.ScreenNameResponse));

        }

        public static void FollowerIds(String screenName, TwitterContext twitterCtx)
        {
            var followers =
               (from follower in twitterCtx.Friendship
                where follower.Type == FriendshipType.FollowerIDs &&
                      follower.ScreenName == screenName
                select follower)
                .SingleOrDefault();

            followers.IDInfo.IDs.ForEach(id =>Console.WriteLine("Follower ID: " + id));
        }

        public static void UserTimeLine(String screenName, TwitterContext twitterCtx)
        {
            var statusTweets =
               from tweet in twitterCtx.Status
               where tweet.Type == StatusType.User
                     && tweet.ScreenName == screenName
               select tweet;

            statusTweets.ToList().ForEach(
                tweet => Console.WriteLine(
                "Name: {0}, Tweet: {1}\n",
                tweet.User.Name, tweet.Text));
        }








    }
}
