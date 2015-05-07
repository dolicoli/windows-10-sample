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
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SocialApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LKLogin : Page
    {
        public string Code;
        public DateTime TokenExpiry;
        public string ClientId = ""; //Put your Client ID Key here
        public string ClientSecret = ""; //Put your Client Secret Key Here
        public string redirecturi = "http://localhost/"; //Put the site you input as redirect uri here
        public LKLogin()
        {
            this.InitializeComponent();
            Login();
        }
        private async Task Login()
        {
            //Linkedin app id
            var clientId = ClientId;
            //Linkedin Scope
            var scope = "r_basicprofile";

            var redirectUri = redirecturi;
            var loginUrl = "https://www.linkedin.com/uas/oauth2/authorization?response_type=code&client_id=" + ClientId + "&redirect_uri=" + redirectUri + "&state=DCEeFWf45A53sdfKef424&scope=" + scope; //state is random string that you need to generate

            Uri startUri = new Uri(loginUrl);
            Uri endUri = new Uri(redirectUri, UriKind.Absolute);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            await ParseAuthenticationResult(result);
        }
        private async Task OAuth2(string code)
        {
            if (code != "")
            {             
                var redirectUri = redirecturi;
                var loginUrl = "https://www.linkedin.com/uas/oauth2/accessToken";
                HttpClient client = new HttpClient();
                HttpStringContent content = new HttpStringContent("grant_type=authorization_code&code=" + code + "&redirect_uri=" + redirectUri + "&client_id=" + ClientId + "&client_secret=" + ClientSecret);
                content.Headers.ContentType = HttpMediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                HttpResponseMessage httpResponseMessage = await client.PostAsync(new Uri(loginUrl), content);
                string response = await httpResponseMessage.Content.ReadAsStringAsync(); //Get the ccess token from here
               
            }
        }
        public async Task ParseAuthenticationResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.ErrorHttp:
                    Debug.WriteLine("Error");
                    break;
                case WebAuthenticationStatus.Success:
                    if (result.ResponseData.Contains("code"))
                    {
                        var pattern = string.Format("{0}\\?code={1}&state={2}", redirecturi, "([a-z0-9A-Z\\-_]+)", "([a-z0-9A-Z]+)");
                        var match = Regex.Match(result.ResponseData, pattern);

                        var code = match.Groups[1];


                        Code = code.Value;
                        OAuth2(Code);
                    }
                    


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
