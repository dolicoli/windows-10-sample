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
using Windows.System.UserProfile;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SocialApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();
            GetImageAndName();
        }

        private async void GetImageAndName()
        {
            if (UserInformation.NameAccessAllowed) //If its not allowed, you can enable it in your privacy settings.
            {
                string firstName = await UserInformation.GetFirstNameAsync();
                if (!string.IsNullOrEmpty(firstName))
                { txtUserName.Text = "Hi, " +  firstName; animationIn.Begin(); }
                StorageFile image = UserInformation.GetAccountPicture(AccountPictureKind.SmallImage) as StorageFile;
                if (image != null)
                {
                    try
                    {
                        IRandomAccessStream imageStream = await image.OpenReadAsync();
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(imageStream);
                        imgUser.Source = bitmapImage;
                        imgUser.Visibility = Visibility.Visible;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fr1.Navigate(typeof(FBLogin), null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            fr1.Navigate(typeof(LKLogin), null);
        }

        private void mainPane_PaneClosed(SplitView sender, object args)
        {
            Hamburger.IsChecked = false;
        }
    }


}
