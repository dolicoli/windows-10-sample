using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guidea.Models
{
    class MainModel
    {
    }

    public class Item
    {
        public string id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string channel { get; set; }
        public string author { get; set; }
        public string publishDate { get; set; }
        public string imageUrl { get; set; }
        public string square3Url { get; set; }
        public string rectangle1Url { get; set; }
        public bool featured { get; set; }
        public int views { get; set; }
        public int favorites { get; set; }
        public string instructableType { get; set; }
    }

    public class ListItem
    {
        public List<Item> items { get; set; }
    }
}
