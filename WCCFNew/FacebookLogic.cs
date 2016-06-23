using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows;
//using System.Dynamic;
using System.IO;
using Facebook;

namespace WCCFNew
{
    class FacebookLogic
    {
        private const string AppId = "1714601905437313"; // FB given app id - Found on Dev Site.
        private const string ExtendedPermissions = "publish_actions"; // Permissions granted to the user
        private string _accessToken; // needed to carry out any tasks
        private string photoPath; // Path for photo
        private bool postSuccess; // True / False for successful post
        //private string postDirection; // Decides where to post the status to
        List<string> postDirectionList = new List<string>(); // List of post directions
        AccessTokenCheck atCheck;
        LoginPG loginScreen;
        QuickPost quickPost;

        // Makes a status post
        private void StatusPost(string fbMessage, bool photoSelectedValue)
        {
            try
            {
                var fb = new FacebookClient(_accessToken);
                quickPost = new QuickPost();
                dynamic result;

                //if (cbGroup.IsChecked == true)
                //{
                //    groupID = File.ReadAllText(@"FacebookID's\groupID.txt") + "/feed";
                //    postDirectionList.Add(groupID);
                //}
                //if (cbWall.IsChecked == true)
                //{
                //    postDirectionList.Add("me/feed");
                //}
                //if (cbPage.IsChecked == true)
                //{
                //    pageID = File.ReadAllText(@"FacebookID's\pageID.txt") + "/feed";
                //    postDirectionList.Add(pageID);
                //}
                //if (cbGroup.IsChecked == false && cbPage.IsChecked == false && cbWall.IsChecked == false)
                //{
                //    postDirectionList.Add(null);
                //}

                //foreach (string item in postDirectionList)
                //{
                //    result = fb.Post(item, new // Change the group id here!!!
                //    {
                //        message = txtMessageFB.Text
                //    });
                //}

                if (photoSelectedValue == true)
                {
                    FacebookMediaObject media = new FacebookMediaObject
                    {
                        FileName = photoPath,
                        ContentType = "image/jpeg"

                    }.SetValue(File.ReadAllBytes(@"" + photoPath));


                    result = fb.Post("me/photos", new
                    {
                        message = fbMessage,
                        source = photoPath,
                        media
                    }
                );
                }
                else
                {
                    result = fb.Post("me/feed", new
                    {
                        message = fbMessage
                    }
                );
                }

                postSuccess = true;
            }
            catch (FacebookOAuthException)
            {
                MessageBoxResult fbAnswer = MessageBox.Show("An error occured. Try loggin back in?", "Error Posting Status", MessageBoxButton.YesNo);
                switch (fbAnswer)
                {
                    case MessageBoxResult.Yes: Login();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // For Facebook
        private void DisplayAppropriateMessage(FacebookOAuthResult facebookOAuthResult)
        {
            // If OAuth Doesnt return Null
            if (facebookOAuthResult != null)
            {
                // If OAuth Authentication was successful
                if (facebookOAuthResult.IsSuccess)
                {
                    // Instatiates the AccessToken Variable and creates a FB Client Object
                    _accessToken = facebookOAuthResult.AccessToken;
                    var fb = new FacebookClient(facebookOAuthResult.AccessToken);

                    // Gets the users name through a query on the Graph API
                    dynamic result = fb.Get("/me");
                    var name = result.name;

                    // Welcomes the user after a successful login
                    MessageBox.Show("Success. Hello " + name);
                    //txtMessageFB.IsEnabled = true;
                    //btnClearFB.IsEnabled = true;
                    //btnSubmitFB.IsEnabled = true;
                    //cbGroup.IsEnabled = true;
                    //cbWall.IsEnabled = true;
                    //cbPage.IsEnabled = true;
                    atCheck = new AccessTokenCheck(_accessToken);
                    _accessToken = atCheck.getExtendedToken;
                    File.WriteAllText(@"AccessTokenStorage\accessToken.txt", _accessToken);
                }
                else
                {
                    // If OAuth fails, a message box will return a short error description.
                    MessageBox.Show(facebookOAuthResult.ErrorDescription);
                }
            }
        }

        // For Facebook if something goes wrong or needs reauthentication
        public void Login()
        {
            var fbLoginDialog = new FacebookLoginDialog(AppId, ExtendedPermissions); // Creates the Facebook login dialog
            fbLoginDialog.ShowDialog(); // Shows the login form

            DisplayAppropriateMessage(fbLoginDialog.facebookOAuthResult); // DisplaysAppropriateMessage for the OAuth Result
        }

        public bool postClick(string message, bool photoSelectedValue, string extractedPhotoPath)
        {
            loginScreen = new LoginPG();
            _accessToken = loginScreen.getStoredToken;
            photoPath = extractedPhotoPath;
            StatusPost(message, photoSelectedValue);
            
            postDirectionList.Clear();
            return postSuccess;
        }

        // For Facebook
        public string accessTokenProp
        {
            set
            {
                _accessToken = value;
            }
        }
    }
}
