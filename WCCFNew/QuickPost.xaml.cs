using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Net;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net.Mail;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using Facebook;

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
        LoginPG loginScreen;
        FacebookLogic fbClass;
        private bool photoSelected;
        private string photoPath;
        //----------------------------------------------------------------------------------------------------

        // Email Variables
        string User;
        string Pass;
        string Sub;
        string Bod;
        //-----------------

        //twitter variable
        List<Twit> twitter;

        private const int MAX_CHARS = 140;
        private const int MAX_PIC_CHARS = 117;
        string dbConnectionString;
        private SEMDBDataContext db = new SEMDBDataContext(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\WCCF Database Test\WCCFNew (MASTER)\WCCFNew\WCCFNew\SMBDB.mdf; Integrated Security = True");

        public QuickPost()
        {
            InitializeComponent();
            fillCombo();
            var tToken = db.ExecuteQuery<string>("SELECT AToken FROM dbo.Twitter");
            var tSecret = db.ExecuteQuery<string>("SELECT ASecret FROM dbo.Twitter");
            var tSN = db.ExecuteQuery<string>("SELECT ScreenName FROM dbo.Twitter");
            var tUID = db.ExecuteQuery<Int64>("SELECT UserId From dbo.Twitter");

            for (int i = 0; i < tToken.Count(); i++)
            {
                twitter.Add(new Twit(tToken.ElementAt(i), tSecret.ElementAt(i), tUID.ElementAt(i), tSN.ElementAt(i)));
            }
        }

        private void fillCombo()
        {

            //Table<SMAccountInfo> accounts = new Table<SMAccountInfo>();
            var fbQuery = db.ExecuteQuery<object>("SELECT * FROM dbo.Face");            
            

            foreach (var item in fbQuery)
            {
                MenuItem temp = new MenuItem();
                temp.IsCheckable = true;
                temp.StaysOpenOnClick = true;
                temp.Header = item.AccountName;
                FacebookAccounts.Items.Add(temp);
            }
            fbQuery = from stuff in db.SMAccountInfos where stuff.SMType == 1 select stuff;
            foreach (var item in fbQuery)
            {
                MenuItem temp = new MenuItem();
                temp.IsCheckable = true;
                temp.StaysOpenOnClick = true;
                temp.Header = item.AccountName;
                TwitterAccounts.Items.Add(temp);
            }
            fbQuery = from stuff in db.SMAccountInfos where stuff.SMType == 3 select stuff;
            foreach (var item in fbQuery)
            {
                MenuItem temp = new MenuItem();
                temp.IsCheckable = true;
                temp.StaysOpenOnClick = true;
                temp.Header = item.AccountName;
                EMailAccounts.Items.Add(temp);
            }
        }

        private void MainMenuBTN_Click(object sender, RoutedEventArgs e)
        {
            MainMenuPG mm = new MainMenuPG();
            this.NavigationService.Navigate(mm);
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
            {
                photoSelected = true;
            }
            else
            {
                photoSelected = false;
            }

            loginScreen = new LoginPG();
            fbClass = new FacebookLogic();
            bool postSuccess = fbClass.postClick(EventText.Text, photoSelected, photoPath);
            // --------------------------------------------------------------------------------------------------------------------------------------


            //twitter posting logic------------------------------
            bool tSuc;
            if (photoPath == null || photoPath.Length<=0)//no picture
            { tSuc = twitter.post(twitterMessage); }
            else//1 picture
            { tSuc = twitter.post(twitterMessage,photoPath); }
            //--------------------------------------------

            // Email Posting Logic
            var eU = db.ExecuteQuery<string>("SELECT UserName FROM dbo.UMail");
            var eP = db.ExecuteQuery<string>("SELECT Password FROM dbo.UMail");
            Sub = "West Chest Community Foundation E-Blast";
            Bod = EventText.Text;
            for (int i = 0; i < eU.Count(); i++)
            {
                var client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(eU.ElementAt(i), eU.ElementAt(i));
                try
                {
                    // Create instance of message
                    MailMessage emailMessage = new MailMessage(); 
                    var emailSendingQuery = db.ExecuteQuery<string>("SELECT EmailAddress FROM dbo.Email");
                    foreach (var item in emailSendingQuery)
                    { emailMessage.Bcc.Add(item); }
                     
                    //Word docs end with .Docx    Excel with .xlsx   Pdf with .pdf   Powerpoint with .pptx   images with appropiate file typ, .jpg, .png, etc.
                    //message.Attachments.Add(new Attachment(@"M:\EmailTester.Docx"));

                    // Set sender
                    emailMessage.From = new MailAddress(eU.ElementAt(i));
                    // Set subject
                    emailMessage.Subject = Sub;
                    //// Set body of message
                    emailMessage.Body = Bod;
                    // Send the message
                    client.Send(emailMessage);
                    // Clean up
                    message = null;
                    Bod = null;
                }
                catch (Exception)
                { }
            }
                        


            //Displays results-----------------------------------------------------------
            MessageBox.Show(string.Format("Success:\nTwitter: {0}\nFacebook: {1}",tSuc,postSuccess), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            
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
    }
}
