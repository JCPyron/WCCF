using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using System.IO;

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for SettingsPG.xaml
    /// </summary>
    public partial class SettingsPG : Page
    {
        Uri myGroupUri, myPageUri;
        string tempGroup, finalGroupID, tempPage, finalPageID;
        string[] groupID;
        string[] pageID;

        SEMDBDataContext db = new SEMDBDataContext(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\WCCF\WCCFNew\SMBDB.mdf;Integrated Security=True");
        #region New Client submission
        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            string address = AddressTxtBox.Text;
            string city = CityTxtBox.Text;
            string state = StateTxtBox.Text;
            string zip = ZipTxtBox.Text;
            string email = EmailTxtBox.Text;
            string twitterHandle = TwitterHandleTxtBox.Text;
            string facebookEmail = FacebookEmailTxtBox.Text;
            User newC = new User
            {
                FirstName = FirstNameTxtBox.Text,
                LastName = LastNameTxtBox.Text,
            };
            db.Users.InsertOnSubmit(newC);
            if (email != null)
            { db.Emails.InsertOnSubmit(new Email { EmailAddress = email, User = newC }); }
            if (facebookEmail != null)
            { db.SocialMedias.InsertOnSubmit(new SocialMedia { User = newC, SMHandle = facebookEmail, SMtyKey = 2 }); }
            if (twitterHandle != null)
            { db.SocialMedias.InsertOnSubmit(new SocialMedia { User = newC, SMHandle = twitterHandle, SMtyKey = 1 }); }
            try { db.SubmitChanges(); }
            catch (Exception ex) {
                StreamWriter w = new StreamWriter("errorLog");
                w.Write(ex.Message + "\n"+"Client Submit" + DateTime.Now +"\n\n");
                MessageBox.Show("AN ERROR HAS OCCURED WHEN SUBMITTING THE CLIENT","Database Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        #endregion

        public SettingsPG()
        {
            InitializeComponent();
        }

        #region Twitter
        Twit newTwitter;
        private void btnTwitterAdd_Click(object sender, RoutedEventArgs e)
        {
            VerSuc.Visibility = Visibility.Hidden;
            newTwitter = new Twit();
        }

        private void btnTwitterVerify_Click(object sender, RoutedEventArgs e)
        {
            try {
                string v = Verify.Text;
                if (v != null || v != "") { newTwitter.authorize(v); }
                db.Twitters.InsertOnSubmit(new Twitter
                {
                    AToken = newTwitter.getAuthTokenAsString().Trim(),
                    ASecret = newTwitter.getAuthSecretAsString(),
                    UserId = newTwitter.getUserID(),
                    ScreenName = newTwitter.getUserHandle()
                });
                db.SubmitChanges();
                VerSuc.Visibility = Visibility.Visible;
            }
            catch (Exception ex) { MessageBox.Show("The verification failed. Try again."); }
            Verify.Text = "";
        }

        
        #endregion

        #region Facebook Group
        // Sets the given url as the posting group (by parsing for group ID)
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myGroupUri = new Uri(txtFBGroupUrl.Text);
                tempGroup = myGroupUri.ToString();
                tempGroup = tempGroup.Substring(32);
                groupID = tempGroup.Split('/');
                finalGroupID = groupID[0];
                File.WriteAllText(@"FacebookID's\groupID.txt", finalGroupID);
                MessageBox.Show("Facebook Group Set! Settings Applied", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                groupID = null;
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Error, please make sure to enter a valid URL", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error has occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Sets the post page with the given url (Through Page ID)
        private void btnPageApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myPageUri = new Uri(txtFBPageUrl.Text);
                tempPage = myPageUri.ToString();
                tempPage = tempPage.Substring(30);
                pageID = tempPage.Split('/');
                finalPageID = pageID[0];
                File.WriteAllText(@"FacebookID's\pageID.txt", finalPageID);
                MessageBox.Show("Facebook Page Set! Settings Applied", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                pageID = null;
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Error, please make sure to enter a valid URL", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error has occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Clears Page Url Text
        private void btnPageClear_Click(object sender, RoutedEventArgs e)
        {
            txtFBPageUrl.Clear();
        }

        // Clears the group url link box
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtFBGroupUrl.Clear();
        }
        #endregion
        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            if (txtPass.Text != "" && txtPass.Text != null && txtUser.Text != "" && txtUser.Text != null)
            db.UMails.InsertOnSubmit(new UMail { UserName = txtUser.Text, Password = txtPass.Text });
            try { db.SubmitChanges(); }
            catch (Exception ex) {
                StreamWriter w = new StreamWriter("errorLog");
                w.Write(ex.Message + "\n"+"Email Submit" + DateTime.Now+"\n\n");
                MessageBox.Show("AN ERROR HAS OCCURED WHEN SUBMITTING THE NEW EMAIL", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
