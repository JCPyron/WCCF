using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Linq;
using System.Data;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for LoginPG.xaml
    /// </summary>
    public partial class LoginPG : Page
    {
        // Facebook Variables -----------------
        private string fbAccessToken;
        FacebookLogic fbClass = new FacebookLogic();
        //-------------------------------------

        //Twitter variables-------------------------
        private bool isAuth = false;
        private Twit temp;
        //------------------------------------
        private SEMDBDataContext db = new SEMDBDataContext(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\WCCF Database Test\WCCFNew (MASTER)\WCCFNew\WCCFNew\SMBDB.mdf; Integrated Security = True");


        public LoginPG()
        {
            InitializeComponent();
            var facebookQ = db.ExecuteQuery<string>("SELECT AToken FROM dbo.Face");
            var twitQ = db.ExecuteQuery<string>("SELECT AToken FROM dbo.Twitter");
            if (facebookQ.Count() == 0) fbClass.Login();
            isAuth = !(twitQ.Count() <= 0);
            if (isAuth)
            { TwitterPB.IsEnabled = false; }
            else
            { temp = new Twit(); }
        }

        // Returns the Email Address
        public string getEmail
        { get { return AddressTXT.Text; } }

        // Returns the Email Password
        public string getEmailPassword
        { get { return EMailPB.Password; } }

        // Returns stored token for Facebook
        public string getStoredToken
        { get { return fbAccessToken; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainMenuPG mainMenu = new MainMenuPG();
            this.NavigationService.Navigate(mainMenu);
        }

        private void ClearBTN_Click(object sender, RoutedEventArgs e)
        {
            AddressTXT.Clear();
            EMailPB.Clear();
            TwitterPB.Clear();
        }

        private void LoginBTN_Click(object sender, RoutedEventArgs e)
        {
            string EMailAdd = AddressTXT.Text;
            string EmailPass = EMailPB.Password;

            if (!isAuth)
            {
                temp.authorize(TwitterPB.Password.ToString().Trim());
                db.Twitters.InsertOnSubmit(new Twitter(){
                    AToken = temp.getAuthTokenAsString(),
                    ASecret = temp.getAuthSecretAsString(),
                    UserId = temp.getUserID(),
                    ScreenName = temp.getUserHandle()
                });
                db.SubmitChanges();
                TwitterPB.Password = "";
                TwitterPB.IsEnabled = false;
            }
           
        }
    }
}
