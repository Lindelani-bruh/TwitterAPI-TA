using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnotherTwitterApplication
{
    class TwitterWrapper
    {
        public void Tweet(string message, TwitterContext twitterCtx)
        {
            var status =
                message + " \n " +
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


        public void FollowerScreenName(String screenName, TwitterContext twitterCtx)
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

        public void FollowerIds(String screenName, TwitterContext twitterCtx)
        {
            var followers =
               (from follower in twitterCtx.Friendship
                where follower.Type == FriendshipType.FollowerIDs &&
                      follower.ScreenName == screenName
                select follower)
                .SingleOrDefault();

            followers.IDInfo.IDs.ForEach(id => Console.WriteLine("Follower ID: " + id));
        }

        public void UserTimeLine(String screenName, TwitterContext twitterCtx)
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
