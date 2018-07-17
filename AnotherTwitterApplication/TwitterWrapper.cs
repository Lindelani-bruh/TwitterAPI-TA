using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAPITA
{
    public class TwitterWrapper
    {
        
        public bool Tweet(string message, TwitterContext twitterCtx)
        {
            var status = message+"";
            var success = false;

            Console.WriteLine("\nStatus being sent: \n\n\"{0}\"", status);
            Console.WriteLine("\nPress any key to post tweet...\n");
            Console.ReadKey();

            try { 
                var tweet = twitterCtx.TweetAsync(status).Result;
                Console.WriteLine(
                        "Status returned: " +
                        "(" + tweet.StatusID + ")" +
                        tweet.User.Name + ", " +
                        tweet.Text + "\n");
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                success = false;
            }

            return success;
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

        public int UserTimeLine(String screenName, TwitterContext twitterCtx)
        {
            const int MaxTweetsToReturn = 20;

            var statusTweets =
               (from tweet in twitterCtx.Status
               where tweet.Type == StatusType.User
                     && tweet.ScreenName == screenName
                     &&  tweet.Count == MaxTweetsToReturn
               select tweet).ToList();

            if (statusTweets.Count > 0) {
                statusTweets.ForEach(tweet => Console.WriteLine("Name: {0}, Tweet: {1}\n",tweet.User.Name, tweet.Text));
            }
            else {
                Console.WriteLine("No entries found.");
            }
            return statusTweets.Count();
        }


        public void UserTimeLine(String screenName, TwitterContext twitterCtx, int TweetsToReturn,int TotalResults)
        {
            int MaxTweetsToReturn = TweetsToReturn;
            int MaxTotalResults = TotalResults;
            // oldest id you already have for this search term
            ulong sinceID = 1;

            // used after the first query to track current session
            ulong maxID;
            var combinedSearchResults = new List<Status>();


            var statusTweets =
               (from tweet in twitterCtx.Status
               where tweet.Type == StatusType.User
                     && tweet.ScreenName == screenName
                     && tweet.Count == MaxTweetsToReturn
                     && tweet.SinceID == sinceID
               select tweet).ToList();

            //List<LinqToTwitter.Status> tweets = statusTweets.ToList();

            if (statusTweets.Count > 0)
            {
                statusTweets.ForEach(
                tweet => Console.WriteLine(
                "Name: {0}, Tweet: {1}\n",
                tweet.User.Name, tweet.Text));
            }
            else
            {
                Console.WriteLine("No entries found.");
            }

            if (statusTweets != null)
            {
                combinedSearchResults.AddRange(statusTweets);
                ulong previousMaxID = ulong.MaxValue;

                do
                {
                    maxID = statusTweets.Min(status => status.StatusID) - 1;
                    Console.WriteLine("=================================================================================");
                    Console.WriteLine("=================================================================================");
                    Console.WriteLine("maxID: {0}, previousMaxID: {1}, ", maxID, previousMaxID);
                    previousMaxID = maxID;

                    statusTweets =
                       (from tweet in twitterCtx.Status
                       where tweet.Type == StatusType.User
                             && tweet.ScreenName == screenName
                             && tweet.Count == MaxTweetsToReturn
                             && tweet.MaxID == maxID
                             && tweet.SinceID == sinceID
                       select tweet).ToList();

                    //tweets = statusTweets.ToList();
                    statusTweets.ForEach(tweet => Console.WriteLine("Name: {0}, Tweet: {1}\n", tweet.User.Name, tweet.Text));
                    combinedSearchResults.AddRange(statusTweets);
                } while (statusTweets.Any() && combinedSearchResults.Count < MaxTotalResults);
            }
            else
            {
                Console.WriteLine("No entries found.");
            }
        }
    }
}
