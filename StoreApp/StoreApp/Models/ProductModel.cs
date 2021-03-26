using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class ProductModel
    {
        public string id { get; set; }
        public string product_title { get; set; }
        public string description { get; set; }
        public double price { get; set; }
        public string availability { get; set; }
        public List<string> specs { get; set; } = new List<string>();
        public int popularity { get; set; }
        public string imagePath { get; set; }

    }
}