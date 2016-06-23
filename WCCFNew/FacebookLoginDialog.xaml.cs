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
using System.Windows.Shapes;
using System.Dynamic;
using Facebook;

namespace WCCFNew
{
    /// <summary>
    /// Interaction logic for FacebookLoginDialog.xaml
    /// </summary>
    public partial class FacebookLoginDialog : Window
    {
        public FacebookLoginDialog(FacebookClient fb, string appId, string extendedPermissions) // Constructor
        {
            if (fb == null) // Throws exception if Client value is null
                throw new ArgumentNullException("fb");
            if (string.IsNullOrWhiteSpace(appId)) // Throws an exception if appId is blank or null
                throw new ArgumentNullException("appId");

            _fb = fb; // Instantiates _fb
            _loginUrl = GenerateLoginUrl(appId, extendedPermissions); // Instantiates the _loginUrl to the Generated login URL

            InitializeComponent();
        }

        public FacebookLoginDialog(string appId, string extendedPermissions) // Another constructor
            : this(new FacebookClient(), appId, extendedPermissions)
        {
        }

        private readonly Uri _loginUrl; // Stores the login URL
        protected readonly FacebookClient _fb; // Creates a Facebook Client

        public FacebookOAuthResult facebookOAuthResult { get; private set; } // OAuth Property

        // Generates a Login Url based on the given appId and Permissions requested
        private Uri GenerateLoginUrl(string appId, string extendedPermissions)
        {
            dynamic parameters = new ExpandoObject(); // ExpandoObject - an object that can be dynamically added and removed at runtime
            parameters.client_id = appId; // Sets the client id equal to the appId
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html"; // Redirects user to the login success screen.

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // Popup - a display node. More can be found at: http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            if (!string.IsNullOrWhiteSpace(extendedPermissions)) // If there are extendedPermissions present, they will be added
                parameters.scope = extendedPermissions;

            // When the form is loaded, navigate to the login url.
            return _fb.GetLoginUrl(parameters);
        }

        private void FacebookLoginDialog_Load(object sender, RoutedEventArgs e)
        {
            // Make sure to use AbsoluteUri.
            webBrowser.Navigate(_loginUrl.AbsoluteUri);
        }

        private void webBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            // whenever the browser navigates to a new url, try parsing the url.
            // the url may be the result of OAuth 2.0 authentication.

            FacebookOAuthResult oauthResult;
            if (_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult)) // If the FB Client parses the url to FacebookOAuthResult (and succeeds)
            {
                facebookOAuthResult = oauthResult; // Insantiates facebookOAuthResult to value of oauthResult
                //DialogResult = facebookOAuthResult.IsSuccess ? DialogResult.OK : DialogResult = false; // Fives DialogResult the value of Ok or No

                bool successful = false;
                if (facebookOAuthResult.IsSuccess)
                {
                    successful = true;
                }

                DialogResult = successful;
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                facebookOAuthResult = null;
            }
        }
    }
}
