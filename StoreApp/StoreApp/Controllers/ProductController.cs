using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLib.Logics;

namespace StoreApp.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {            
            List<ProductModel> products = new List<ProductModel>();

            products = getProducts();
            return View(products);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            var products = getProducts();

            var actionProduct = products.Where(x => x.id == id).FirstOrDefault();

            return View(actionProduct);
        }

        [HttpPost]
        public JsonResult GetAvailability(LabelClass lblData)
        {
            string productId = lblData.LabelId.Replace("lb_","");
            var products = getProducts();
            var actionProduct = products.Where(x => x.id == productId).FirstOrDefault();

            return Json(actionProduct.availability, JsonRequestBehavior.AllowGet);
        }

        private static List<ProductModel> getProducts()
        {
            var products = (from p in ProductProcessor.GetProducts()
                        select new ProductModel
                        {
                            id = p.id,
                            product_title = p.product_title,
                            description = p.description,
                            price = p.price,
                            availability = p.availability,
                            specs = p.specs,
                            popularity = p.popularity,
                            imagePath = p.imagePath
                        }).ToList();

            return products;
        }
    }

    public class LabelClass
    {
        public string LabelId { get; set; }
        
    }
}