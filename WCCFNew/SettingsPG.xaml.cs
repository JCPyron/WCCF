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

        SEMDBDataContext db = new SEMDBDataContext();

        public SettingsPG()
        {
            InitializeComponent();
        }

        #region New Client submission
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTxtBox.Text;
            string lastName = LastNameTxtBox.Text;
            string address = AddressTxtBox.Text;
            string city = CityTxtBox.Text;
            string state = StateTxtBox.Text;
            string zip = ZipTxtBox.Text;
            string email = EmailTxtBox.Text;
            string twitterHandle = TwitterHandleTxtBox.Text;
            string facebookEmail = FacebookEmailTxtBox.Text;
            User u = new User();
            u.FirstName = firstName;
            u.LastName = lastName;

            u.address.StreetAddress = address;
            u.address.city = city;
            u.address.state = state;
            u.address.zip = Convert.ToInt32(zip);
            if (email != null || facebookEmail != null || twitterHandle != null)
            {

                if (email != null)
                {
                    u.Email.EmailAddress = email;
                }

                if (facebookEmail != null)
                {
                    SocialMedia s = new SocialMedia();
                    s.User = u;
                    s.User_UserID = s.User.UserID;
                    s.SMtyKey = 2;
                    u.SocialMedias.Add(s);
                }
                if (twitterHandle != null)
                {
                    SocialMedia s = new SocialMedia();
                    s.User = u;
                    s.User_UserID = s.User.UserID;
                    s.SMtyKey = 1;
                    u.SocialMedias.Add(s);
                }

            }

            db.Users.InsertOnSubmit(u);
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
    }
}
