using Guidea.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Guidea
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static string MashapeKey = ""; // your key goes here
        public MainPage()
        {
            this.InitializeComponent();
            GetListItems();
            //Windows.Storage.ApplicationDataContainer container = Windows.Storage.ApplicationData.Current.RoamingSettings;
            //Windows.Storage.StorageFolder folder = Windows.Storage.ApplicationData.Current.RoamingFolder;
            
        }

        public async void GetListItems()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Mashape-Key", MashapeKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpResponseMessage response = await client.GetAsync("https://devru-instructables.p.mashape.com/list?limit=20&offset=0&sort=recent&type=id");
            string content = await response.Content.ReadAsStringAsync();
            ListItem items = new ListItem();
            items.items = new List<Item>();
            JsonObject job = JsonObject.Parse(content);
            JsonArray jarray = job["items"].GetArray();
            foreach (JsonValue obj in jarray)
            {
                Item item = new Item();
                item.author = obj.GetObject()["author"].ToString();
                item.imageUrl = obj.GetObject()["imageUrl"].ToString();
                item.category = obj.GetObject()["category"].ToString();
                items.items.Add(item);
            }
            listView.ItemsSource = items.items;

        }



       // Task<HttpResponse<MyClass>> response = Unirest.get("https://devru-instructables.p.mashape.com/list?limit=20&offset=0&sort=recent&type=id")
//.header("X-Mashape-Key", "m5ekGWIhFymshucDa6pdlSrU5Y2tp1g7xkijsn6etfHd6y8BLH")
//.header("Accept", "application/json")
//.asJson();





    }
}
