using Facebook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FBLogin : Page
    {
        public string AccessToken;
        public DateTime TokenExpiry;
        public string ClientId = ""; //Put your Client ID Key here
        public string redirecturi = "http://localhost/"; //Put the site you input as redirect uri here
        public FBLogin()
        {
            this.InitializeComponent();
            Login();
        }

        private async Task Login()
        {
            //Facebook app id
            var clientId = ClientId;
            //Facebook permissions
            var scope = "public_profile, email";

            var redirectUri = redirecturi;
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = clientId,
                redirect_uri = redirectUri,
                response_type = "token",
                scope = scope
            });

            Uri startUri = loginUrl;
            Uri endUri = new Uri(redirectUri, UriKind.Absolute);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            await ParseAuthenticationResult(result);

        }

        public async Task ParseAuthenticationResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.ErrorHttp:
                    Debug.WriteLine("Error");
                    break;
                case WebAuthenticationStatus.Success:
                    var pattern = string.Format("{0}\\?#access_token={1}&expires_in={2}", redirecturi, "([a-z0-9A-Z]+)", "([0-9]+)");
                    var match = Regex.Match(result.ResponseData, pattern);

                    var access_token = match.Groups[1];
                    var expires_in = match.Groups[2];

                    AccessToken = access_token.Value;
                    TokenExpiry = DateTime.Now.AddSeconds(double.Parse(expires_in.Value));

                    break;
                case WebAuthenticationStatus.UserCancel:
                    Debug.WriteLine("Operation aborted");
                    break;
                default:
                    break;
            }
        }
    }
}
