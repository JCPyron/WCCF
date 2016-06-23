using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for TwitterPG.xaml
    /// </summary>
    public partial class TwitterPG : Page
    {
        private Twit twitter;
        private string currentACName;
        private const int MAX_POST = 140;
        private const int MAX_PIC = 117;

        public TwitterPG()
        { 
            string[] temp = File.ReadAllLines("TAT.txt");
            twitter = new Twit(temp[0], temp[1], Convert.ToInt64(temp[2]), temp[3]);
            currentACName = twitter.getUserHandle();

            InitializeComponent();
            updateAccountItems();
            comDM.ItemsSource = twitter.getFollowerNames();
        }
        
        //account tab----------------------------------------------------------------------------------------------------
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            currentACName = txtNew.Text;
            updateAccountItems();
        }

        private void btnFollow_Click(object sender, RoutedEventArgs e)
        { twitter.followUser(currentACName); }

        private void btnUnfollow_Click(object sender, RoutedEventArgs e)
        { twitter.unfollowUser(currentACName); }
        

        private void updateAccountItems()
        {
            lblF.Content = "Followers: "+ twitter.getUserFollowerCount(currentACName);
            lblT.Content = "Tweets: " + twitter.getUserTweetCount(currentACName);
            lblUName.Content = twitter.getUserName(currentACName);
            txtNew.Text = "";
            lstFollowers.ItemsSource = twitter.getFollowerNames(currentACName);
            lstTweet.ItemsSource = twitter.getTweetsAsArray(currentACName);
            btnFollow.Content = "Follow: " + twitter.getUserName(currentACName);
            btnUnfollow.Content = "Unfollow: "+twitter.getUserName(currentACName);
        }
        //-----------------------------------------------------------------------------------------------------

        //Post tab---------------------------------------------------------------------------------------------
        private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            if (twitter.post(txtPost.Text))
            {
                MessageBox.Show("Success!", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                txtPost.Text = "";
            }
            else { MessageBox.Show("Something seems to have gone wrong....\nError", "Result", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void txtPost_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPost.MaxLength = MAX_POST;


            if (txtPost.Text.Length >= MAX_POST)
            {
                string charLim = txtPost.Text.Substring(0, MAX_POST);
                txtPost.Text = charLim;
                lblChars.Foreground = Brushes.Red;
                btnPost.BorderBrush = Brushes.Red;
                lblChars.Content = "Characters remaining: 0";
            }
            else
            {

                lblChars.Foreground = Brushes.Green;
                btnPost.BorderBrush = Brushes.Green;
                lblChars.Content = "Characters remaining: " + Convert.ToString(MAX_POST - txtPost.Text.Length);
            }
        }
        //---------------------------------------------------------------------------------

        //Picture Post Tab--------------------------------------------------------------------------
        private string photoPath;
        private void txtPicPost_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPicPost.MaxLength = MAX_PIC;

            if (txtPicPost.Text.Length >= MAX_PIC)
            {
                string charLim = txtPicPost.Text.Substring(0, MAX_PIC);
                txtPicPost.Text = charLim;
                lblPicChar.Foreground = Brushes.Red;
                btnPicPost.BorderBrush = Brushes.Red;
                lblPicChar.Content = "Characters remaining: 0";
            }
            else
            {

                lblPicChar.Foreground = Brushes.Green;
                btnPicPost.BorderBrush = Brushes.Green;
                lblPicChar.Content = "Characters remaining: " + Convert.ToString(MAX_PIC - txtPicPost.Text.Length);
            }
        }

        private void btnPicPost_Click(object sender, RoutedEventArgs e)
        {
            if(twitter.post(txtPicPost.Text, photoPath))
            {
                txtPicPost.Text = "";
                photoPath = "";
                MessageBox.Show("Success!","Result",MessageBoxButton.OK,MessageBoxImage.Information);
            }
            else { MessageBox.Show("Something seems to have gone wrong....\nError", "Result", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void btnBrowsePic_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filer for file extension and default file extension
            //dlg.Filter = "JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|GIF Files (*.gif)|*.gif";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg"; // JPG and JPEG files are the only ones usable right now

            // Display OpenFileDialog by calling ShowDialog method
            bool result = (bool) dlg.ShowDialog();

            // Get the selected file name and display in a Textbox
            if (result)
            {
                // Open Document
                photoPath = dlg.FileName;
                lblPicSelect.Foreground = Brushes.Green;
                lblPicSelect.Content = "Picture(s) Selected";
                img1.Source = new BitmapImage(new Uri(photoPath, UriKind.RelativeOrAbsolute));
            }
            else
            {
                lblPicSelect.Content = "No Picture Selected";
                lblPicSelect.Foreground = Brushes.Red;
            }
        }
        //-----------------------------------------------------------------------------------------------------------

        //------DM Tab-----------------------------------------

        private string DMtarget;
        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            twitter.directMessage(DMtarget, txtDM.Text);

            btnSelectLock.IsEnabled = true;
            comDM.IsEnabled = true;
            txtDM.IsEnabled = true;
            btnSearch.IsEnabled = true;
            txtTarget.Text = "";
        }
        
        private void btnTargetSearch_Click(object sender, RoutedEventArgs e)
        {
            DMtarget = txtTarget.Text;
            btnSelectLock.IsEnabled = false;
            comDM.IsEnabled = false;
            btnSend.BorderBrush = Brushes.Green;
        }

        private void btnSelectLock_Click(object sender, RoutedEventArgs e)
        {
            DMtarget = comDM.Text;
            txtDM.IsEnabled = false;
            btnSearch.IsEnabled = false;
            btnSend.BorderBrush = Brushes.Green;
        }
        //-------------------------------------------------------------------------------------------------------------

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            currentACName = twitter.getUserHandle();
            updateAccountItems();

            txtPicPost.Text = "";
            photoPath = "";

            txtPost.Text = "";

            btnSelectLock.IsEnabled = true;
            comDM.IsEnabled = true;
            txtDM.IsEnabled = true;
            btnSearch.IsEnabled = true;
            txtTarget.Text = "";
        }
    }
}
