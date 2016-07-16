using System;
using System.Collections.Generic; 
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.IO;
using System.Windows.Media;
using System.Data.Linq;
using System.Net.Mail;
using System.Windows.Navigation;
using Microsoft.Win32; 

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for QuickPost.xaml
    /// </summary>
    public partial class QuickPost : Page
    {
        // Email - Additions to be made.
        // Facebook - Finished.
        // Twitter - Needs to be added in.

        // Facebook Variables -------------------------------------------------------------------------------
        List<FacebookLogic> fbClass = new List<FacebookLogic>();
        private bool photoSelected;
        private string photoPath;
        //----------------------------------------------------------------------------------------------------

        // Email Variables
        List<GmailClass> gMail = new List<GmailClass>();
        string Sub;
        //-----------------

        //twitter variable
        List<Twit> twitter = new List<Twit>();

        private const int MAX_CHARS = 140;
        private const int MAX_PIC_CHARS = 117;
        private static string dbConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\SMBDB.mdf;Integrated Security=True;Connect Timeout=30";
        private SEMDBDataContext db;

        public QuickPost()
        {
            InitializeComponent();
            
            db =new SEMDBDataContext(dbConnectionString);
            //TWITTER: retrieving the db info, setting up the twit classes, and putting them in a list
            try {
                Table<Twitter> t = db.GetTable<Twitter>();
                foreach (Twitter item in t)
                { twitter.Add(new Twit(item.AToken, item.ASecret, item.UserId, item.ScreenName.Trim())); }
            }
            catch (Exception ex)
            {
                StreamWriter w = new StreamWriter("errorLog.txt");
                w.Write(ex.Message + "\n"+"Twitter" + DateTime.Now+"\n\n");
                w.Close();
                MessageBox.Show("AN ERROR HAS OCCURED WHEN PULLING TWITTER DATA", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //FACEBOOK: retrieves the db info and adds it to a list
            try {
                Table<Face> face = db.GetTable<Face>();
                foreach (Face fb in face)
                { fbClass.Add(new FacebookLogic(fb.AToken)); }
            }
            catch (Exception ex)
            {
                StreamWriter w = new StreamWriter("errorLog.txt");
                w.Write(ex.Message + "\n" + "Facebook" + DateTime.Now + "\n\n");
                w.Close();
                MessageBox.Show("AN ERROR HAS OCCURED WHEN PULLING FACEBOOK DATA", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //EMAIL: retrieves the db info
            try {
                Table<UMail> m = db.GetTable<UMail>();
                foreach (UMail temp in m)
                { gMail.Add(new GmailClass(temp.UserName, temp.Password)); }
            }
            catch (Exception ex)
            {
                StreamWriter w = new StreamWriter("errorLog.txt");
                w.Write(ex.Message + "\n" + "Email" + DateTime.Now + "\n\n");
                MessageBox.Show("AN ERROR HAS OCCURED WHEN PULLING EMAIL DATA", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            fillCombo();
        }

        private void fillCombo()
        {

            //Table<SMAccountInfo> accounts = new Table<SMAccountInfo>();         
            
            //facebook check boxes
            foreach (var item in fbClass)
            {
                MenuItem temp = new MenuItem();
                temp.IsCheckable = true;
                temp.StaysOpenOnClick = true;
                temp.Header = item.getUserName();
                FacebookAccounts.Items.Add(temp);
            }
            
            //twitter checkboxes
            foreach (var item in twitter)
            {
                MenuItem temp = new MenuItem();
                temp.IsCheckable = true;
                temp.StaysOpenOnClick = true;
                temp.Header = item.getUserHandle();
                TwitterAccounts.Items.Add(temp);
            }
            

            //email checkboxes
            foreach (var item in gMail)
            {
                MenuItem temp = new MenuItem();
                temp.IsCheckable = true;
                temp.StaysOpenOnClick = true;
                temp.Header = item.User;
                EMailAccounts.Items.Add(temp);
            }
        }

        private void MainMenuBTN_Click(object sender, RoutedEventArgs e)
        {
            MainMenuPG mm = new MainMenuPG();
            NavigationService.Navigate(mm);
        }

        private void ClearMSGBTN_Click(object sender, RoutedEventArgs e)
        {
            EventText.Clear();
            TwitterText.Clear();
            photoPath = null;
            lblPhotoSelected.Visibility = Visibility.Hidden;
        }

        private void SendMSGBTN_Click(object sender, RoutedEventArgs e)
        {
            string message = EventText.Text;
            string twitterMessage = TwitterText.Text;
            //string postSuc = "";
            // Facebook Posting Logic --------------------------------------------------------------------------------------------------------------
            if (lblPhotoSelected.Visibility == Visibility.Visible)
            { photoSelected = true; }
            else
            { photoSelected = false; }

            bool postSuccess = true;
            foreach (FacebookLogic item in fbClass)
            { postSuccess = item.postClick(message, photoSelected, photoPath);  }
            // --------------------------------------------------------------------------------------------------------------------------------------


            //twitter posting logic------------------------------
            bool tSuc = true;
            foreach (Twit item in twitter)
            {
                if (photoPath == null || photoPath.Length <= 0)//no picture
                { tSuc = item.post(twitterMessage); }
                else//1 picture
                { tSuc = item.post(twitterMessage, photoPath); }
            }
            
            //--------------------------------------------

            // Email Posting Logic
            Sub = "West Chest Community Foundation E-Blast";
            foreach (var item in gMail)
            { 
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(item.User, item.Password);
                try
                {
                    // Create instance of message
                    MailMessage emailMessage = new MailMessage(); 
                    var emailSendingQuery = db.ExecuteQuery<string>("SELECT EmailAddress FROM dbo.Email");
                    foreach (var temp in emailSendingQuery)
                    { emailMessage.Bcc.Add(temp); }
                     
                    //Word docs end with .Docx    Excel with .xlsx   Pdf with .pdf   Powerpoint with .pptx   images with appropiate file typ, .jpg, .png, etc.
                    //message.Attachments.Add(new Attachment(@"M:\EmailTester.Docx"));

                    // Set sender
                    emailMessage.From = new MailAddress(item.User);
                    // Set subject
                    emailMessage.Subject = Sub;
                    //// Set body of message
                    emailMessage.Body = message;
                    // Send the message
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    StreamWriter w = new StreamWriter("errorLog.txt");
                    w.Write(ex.Message + "\n" + "Email" + DateTime.Now + "\n\n");
                    w.Close();
                    MessageBox.Show("Email failed to send.","Email failed",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }



            //Displays results-----------------------------------------------------------
            if (tSuc && postSuccess) { MessageBox.Show("Both Twitter and Facebook posted successfully",
                "Success", MessageBoxButton.OK, MessageBoxImage.Information);}
            else if (tSuc) { MessageBox.Show("Something went wrong with Facebook, but Twitter Succeeded.",
                "Partial Success",MessageBoxButton.OK,MessageBoxImage.Exclamation);}
            else if (postSuccess) { MessageBox.Show("Something went wrong with Twitter, but Facebook Succeeded.",
                "Partial Success", MessageBoxButton.OK, MessageBoxImage.Exclamation); }
            else MessageBox.Show("neither Facbook nor Twitter worked", 
                "Critical Failure", MessageBoxButton.OK,MessageBoxImage.Error);

        }

        private void TwitterText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TwitterText.MaxLength = MAX_CHARS;


            if (TwitterText.Text.Length >= MAX_CHARS)
            {
                string charLim = TwitterText.Text.Substring(0, 140);
                TwitterText.Text = charLim;
                RemaininChar.Foreground = Brushes.Red;
                RemaininChar.Content = "Characters remaining: 0";
            }
            else
            {

                RemaininChar.Foreground = Brushes.LimeGreen;
                RemaininChar.Content = "Characters remaining: " + Convert.ToString( MAX_CHARS - TwitterText.Text.Length);
            }


        }

        private void EventText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TwitterText.MaxLength = MAX_CHARS;

            TwitterText.Text = EventText.Text;

        if (TwitterText.Text.Length >= MAX_CHARS)
            {
                string charLim = EventText.Text.Substring(0, 140);
                TwitterText.Text = charLim;
                RemaininChar.Foreground = Brushes.Red;
                RemaininChar.Content = "Characters remaining: 0";
            }
            else
            {
                
                RemaininChar.Foreground = Brushes.LimeGreen;
                RemaininChar.Content = "Characters remaining: " + Convert.ToString(TwitterText.MaxLength - EventText.Text.Length);
            }


        }

        private void btnSelectPhoto_Click(object sender, RoutedEventArgs e)
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
                lblPhotoSelected.Visibility = Visibility.Visible;
            }
            else
            {
                lblPhotoSelected.Visibility = Visibility.Hidden;
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPG sp = new SettingsPG();
            NavigationService.Navigate(sp);
        }
    }
}
