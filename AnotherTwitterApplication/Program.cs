using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterAPITA
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


            TwitterWrapper tw = new TwitterWrapper();

            var twitterCtx = new TwitterContext(auth);

            //APISettings.ReadAllSettings();
            //FollowerIds("OtherHoe",twitterCtx);
            //FollowerScreenName("OtherHoe", twitterCtx);
            //Tweet("Pizza for breakfast and ??.", twitterCtx);
            //tw.UserTimeLine("OtherHoe", twitterCtx);
            //tw.UserTimeLine("OtherHoe", twitterCtx, 10, 100);

            //followers.IDInfo.IDs.ForEach(id =>Console.WriteLine("Follower ID: " + id));

            //Console.WriteLine(followers.IDInfo.IDs.Count());

        }
    }
}
