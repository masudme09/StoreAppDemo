using DataLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataLib.Logics
{
    public static class ProductProcessor
    {
        public static List<ProductModel> GetProducts(string listXmlPath="", string detailXmlPath ="")
        {
            List<ProductModel> products = new List<ProductModel>();
            if (listXmlPath == "" && detailXmlPath =="")
            {
                XDocument listXmlDoc, detailXmlDoc;
                try
                {
                    listXmlDoc = XDocument.Parse(DataLib.Properties.Resources.List);
                    detailXmlDoc = XDocument.Parse(DataLib.Properties.Resources.Detail);
                    products = Utility.getProducts(listXmlDoc, detailXmlDoc);
                }
                catch
                {
                    Console.WriteLine("XML Parsing Error");
                }               
                                
            }
            else
            {
                XDocument listXmlDoc, detailXmlDoc;
                try
                {
                    listXmlDoc = XDocument.Load(listXmlPath);
                    detailXmlDoc = XDocument.Load(detailXmlPath);
                    products = Utility.getProducts(listXmlDoc, detailXmlDoc);
                }
                catch
                {
                    Console.WriteLine("XML Parsing Error");
                }            
                             
            }          

            return products;
        }
    }

    public static class Utility
    {
        public static List<ProductModel> getProducts(XDocument listXML, XDocument detailXML)
        {
            List<ProductModel> products = new List<ProductModel>();
            
            products = (from p in listXML.Descendants("Store").Descendants("Products").Descendants("Product")
                        select new ProductModel()
                        {
                            id = ((string)p.Attribute("id").Value).Trim(),
                            product_title = ((string)p.Element("Title")).Trim(),
                            description = ((string)p.Element("Description")).Trim(),
                            imagePath = ((string)p.Element("Image")).Trim(),
                            price = ((double)p.Element("Price")),
                            popularity = (from s in p.Descendants("Sorting")
                                          select (int)s.Element("Popular")).FirstOrDefault()
                        }).ToList();

            var detailProducts = (from p in detailXML.Descendants("Store").Descendants("Products").Descendants("Product")
                                  select new ProductModel()
                                  {
                                      id = ((string)p.Attribute("id").Value).Trim(),
                                      product_title = ((string)p.Element("Title")).Trim(),
                                      description = ((string)p.Element("Description")).Trim(),
                                      imagePath = ((string)p.Element("Image")).Trim(),
                                      availability = ((string)p.Element("Availability")),
                                      specs = (from s in p.Descendants("Specs").Descendants("Spec")
                                               select (string)s.Value).ToList()
                                  }).ToList();


            //var specstest = (from p in detailXML.Descendants("Store").Descendants("Products").Descendants("Product").Descendants("Specs").)



            //Merging to two list to complete producModel
            /*It is considered that product id should be unique, if duplicate id present in the xml 
            then only first one with the corresponding id taken */
            products = products.Concat(detailProducts)
                .ToLookup(p => p.id)
                .Select(g => g.Aggregate((p1, p2) => new ProductModel
                {
                    id = p1.id,
                    product_title = p1.product_title,
                    description = p1.description,
                    imagePath = p1.imagePath,
                    price = p1.price,
                    popularity = p1.popularity, 
                    availability = p2.availability,
                    specs = p2.specs

                })).ToList();


            return products;
        }
    }
}
