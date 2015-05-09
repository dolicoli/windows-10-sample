using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace Demo1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            GetData();
        }

        private async void GetData()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-Mashape-Key", "some key");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpResponseMessage response = await client.GetAsync("https://devru-instructables.p.mashape.com/list?limit=20&offset=0&sort=recent&type=id");
            string jsonresult = await response.Content.ReadAsStringAsync();
            Windows.Data.Json.JsonObject obj = JsonObject.Parse(jsonresult);
            JsonArray array = obj.GetObject()["items"].GetArray();
            ListItems list = new ListItems();
            list.items = new List<Item>();

            foreach (JsonValue val in array)
            {
                Item newitem = new Item();
                newitem.title = val.GetObject()["title"].GetString();
                newitem.category = val.GetObject()["category"].GetString();
                newitem.imageUrl = val.GetObject()["imageUrl"].GetString();
                list.items.Add(newitem);
            }
            listView.ItemsSource = list.items;


        }

        //Task<HttpResponse<MyClass>> response = Unirest.get("https://devru-instructables.p.mashape.com/list?limit=20&offset=0&sort=recent&type=id")
//.header("X-Mashape-Key", "m5ekGWIhFymshucDa6pdlSrU5Y2tp1g7xkijsn6etfHd6y8BLH")
//.header("Accept", "application/json")
//.asJson();
    }
}
