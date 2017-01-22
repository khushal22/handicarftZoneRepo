using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.MyDB;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ManageProductController : ApiController
    {
        hczdbEntities db = new hczdbEntities();

        public List<tbl_categories> GetCategories()
        {
            var data = from asd in db.tbl_categories select asd;
           
            List<tbl_categories> li = new List<tbl_categories>();

            foreach (var item in data)
            {
                tbl_categories c = new tbl_categories();
                c.cat_id= item.cat_id;
                c.cat_name = item.cat_name;
                c.cat_image = item.cat_image;
                li.Add(c);
            }
            return li;

        }

        public List<tbl_subcategories> GetSubCategories(decimal cat_id1)
        {
            var data = from subCat in db.tbl_subcategories
                       where subCat.cat_id == cat_id1
                       select subCat;

            List<tbl_subcategories> li = new List<tbl_subcategories>();

            foreach (var item in data)
            {
                tbl_subcategories s = new tbl_subcategories();
                s.subcat_id = Convert.ToDecimal(item.subcat_id);
                s.cat_id = item.cat_id;
                s.subcat_name = item.subcat_name;
                s.subcat_image = item.subcat_image;


                li.Add(s);
            }

            return li;
        }
        public List<ProductListClass> GetProductList(decimal subCat)
        {
            var data = from product in db.tbl_productDetail
                       join user in db.tbl_user on product.user_id equals user.user_id
                       where product.subcat_id == subCat
                       select new { product.ad_id, product.cat_id,product.subcat_id, product.user_id, product.pro_name, product.pro_image, product.pro_price,product.pro_desc, product.ad_status, user.user_name, user.user_email, user.user_city, user.user_mob_no  };

            List<ProductListClass> li = new List<ProductListClass>();

            foreach (var item in data)
            {
                ProductListClass p = new ProductListClass();
                p.ProductAdId = item.ad_id;
                p.ProductCatId = Convert.ToDecimal(item.cat_id);
                p.ProductSubCatId = Convert.ToDecimal(item.subcat_id);
                p.UserId = Convert.ToDecimal(item.user_id);
                p.ProductName = item.pro_name;
                p.ProductImage = item.pro_image;
                p.ProductPrice = Convert.ToDecimal(item.pro_price);
                p.ProductDesc = item.pro_desc;
                p.ProductAdStatus = item.ad_status;
               
                p.UserName = item.user_name;
                p.UserEmail = item.user_email;
                p.UserCity = item.user_city;
                p.UserMob = item.user_mob_no;

                li.Add(p);
            }

            return li;
        }



        public List<ProductListClass> GetUsersProducts(decimal userId)
        {
            var data = from product in db.tbl_productDetail
                       join user in db.tbl_user on product.user_id equals user.user_id
                       where product.user_id == userId
                       select new { product.ad_id, product.cat_id, product.subcat_id, product.user_id, product.pro_name, product.pro_image, product.pro_price, product.pro_desc, product.ad_status, user.user_name, user.user_email, user.user_city, user.user_mob_no };

            List<ProductListClass> li = new List<ProductListClass>();

            foreach (var item in data)
            {
                ProductListClass p = new ProductListClass();
                p.ProductAdId = item.ad_id;
                p.ProductCatId = Convert.ToDecimal(item.cat_id);
                p.ProductSubCatId = Convert.ToDecimal(item.subcat_id);
                p.UserId = Convert.ToDecimal(item.user_id);
                p.ProductName = item.pro_name;
                p.ProductImage = item.pro_image;
                p.ProductPrice = Convert.ToDecimal(item.pro_price);
                p.ProductDesc = item.pro_desc;
                p.ProductAdStatus = item.ad_status;

                p.UserName = item.user_name;
                p.UserEmail = item.user_email;
                p.UserCity = item.user_city;
                p.UserMob = item.user_mob_no;

                li.Add(p);
            }

            return li;
        }


        [HttpGet]
        public string SetStatus(string status, decimal adId)
        {
          //  decimal adId = 233;
            var data =( from product in db.tbl_productDetail
                       where product.ad_id == adId
                       select product).FirstOrDefault();

            data.ad_status = status;
            
            db.SaveChanges();
            return "Status Updated";
        }

        public decimal GetNextProduct()
        {

            decimal NextProduct = db.tbl_productDetail.Max(p => p.ad_id);
            return ++NextProduct;
        }
        [HttpGet]
        public string insertProduct(decimal catId,decimal SubCatId,decimal userId,string productName,string productImage,decimal productPrice,string productDesc,string productStatus)
        {
          decimal adId=  GetNextProduct();

            tbl_productDetail p = new tbl_productDetail();
            p.ad_id = adId;
            p.cat_id =catId;
            p.subcat_id =SubCatId;
            p.user_id =userId;
            p.pro_name =productName;
            p.pro_image =productImage;
            p.pro_price =productPrice;
            p.pro_desc =productDesc;
            p.ad_status =productStatus;

            db.tbl_productDetail.Add(p);
            db.SaveChanges();
            return "Product Inserted";
        }
        [HttpGet]
        public string addToWishList(decimal adId,decimal userId)
        {
            tbl_wishList w = new tbl_wishList();
            w.ad_id = adId;
            w.user_id = userId;
            db.tbl_wishList.Add(w);
            db.SaveChanges();
            return "Item Added to Wish List";
        }
        [HttpGet]
        public List<ProductListClass> GetWishList(decimal userId)
        {
            var data = from product in db.tbl_productDetail
                       join customer in db.tbl_user on product.user_id equals customer.user_id
                       join wlist in db.tbl_wishList on product.ad_id equals wlist.ad_id
                       where wlist.user_id==userId
                       select new { product.pro_name, product.pro_desc, product.ad_id, product.pro_price, product.ad_status, product.pro_image, product.user_id, customer.user_name, customer.user_mob_no, customer.user_city, customer.user_email };

            List<ProductListClass> li = new List<ProductListClass>();

            foreach (var item in data)
            {
                
                ProductListClass t = new ProductListClass();
                t.ProductAdId = item.ad_id;
                t.ProductName = item.pro_name;
                t.ProductImage = item.pro_image;
                t.ProductPrice = Convert.ToDecimal(item.pro_price);
                t.ProductDesc = item.pro_desc;
                t.ProductAdStatus = item.ad_status;
                t.UserId = Convert.ToDecimal(item.user_id);
                t.UserName = item.user_name;
                t.UserEmail = item.user_email;
                t.UserCity = item.user_city;
                t.UserMob = item.user_mob_no;

                li.Add(t);
            }

            return li;
        }


    }
}
