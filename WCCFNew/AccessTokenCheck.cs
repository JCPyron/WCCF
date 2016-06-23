using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.IO;
using Facebook;

namespace WCCFNew
{
    class AccessTokenCheck
    {
        // This is the fixed version of the program
        private string accessToken;
        private string extendedAccessToken;

        public AccessTokenCheck(string token)
        {
            accessToken = token;
        }

        /// <summary>
        /// Returns the new access token
        /// </summary>
        public string getExtendedToken
        {
            get
            {
                return GetExtendedAccessToken(accessToken);
            }
        }

        /// <summary>
        /// Returns the amount of time the access token has until expiration
        /// </summary>
        public string getTokenTime
        {
            get
            {
                getAppToken();
                return getTime();
            }
        }

        /// <summary>
        /// Extends AccessToken timeout from 1-2 Hours to 60 Days
        /// </summary>
        /// <param name="ShortLivedToken"></param>
        /// <returns>Returns new accessToken</returns>
        private string GetExtendedAccessToken(string ShortLivedToken)
        {
            FacebookClient client = new FacebookClient();
            string extendedToken = "";
            try
            {
                dynamic result = client.Get("/oauth/access_token", new
                {
                    grant_type = "fb_exchange_token",
                    client_id = "1714601905437313",
                    client_secret = "f1cfbbb977532db9c07f1d0038380841",
                    fb_exchange_token = ShortLivedToken
                });
                extendedToken = result.access_token;
            }
            catch
            {
                extendedToken = ShortLivedToken;
            }
            extendedAccessToken = extendedToken;
            return extendedToken;
        }

        /// <summary>
        /// Gets the App Access Token needed for expiration time
        /// </summary>
        /// <returns></returns>
        private string getAppToken()
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = "1714601905437313",
                client_secret = "f1cfbbb977532db9c07f1d0038380841",
                grant_type = "client_credentials"
            });

            return result.access_token;
        }

        /// <summary>
        /// Gets the amount of time left on the access token until its expiration
        /// </summary>
        /// <returns></returns>
        private string getTime()
        {
            var fb = new FacebookClient();
            dynamic result = fb.Get("debug_token", new
            {
                access_token = getAppToken(),
                input_token = extendedAccessToken
            });
            return result;
        }
    }
}
