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
    /// <summary>
    /// 
    /// </summary>
    public class ManageUserController : ApiController
    {

        hczdbEntities db = new hczdbEntities();

        public List<tbl_user> GetUser(decimal UserId)
        {
            var data = from user in db.tbl_user where user.user_id == UserId select user;

            List<tbl_user> li = new List<tbl_user>();

            foreach (var item in data)
            {
                tbl_user u = new tbl_user();
                u.user_id = item.user_id;
                u.user_name = item.user_name;
                u.user_email = item.user_email;
                u.user_add = item.user_add;
                u.user_locality = item.user_locality;
                u.user_city = item.user_city;
                u.user_pincode = item.user_pincode;
                u.user_mob_no = item.user_mob_no;
                li.Add(u);
            }
            return li;

        }
        [HttpGet]
        public string login(string Id, string Pass)
        {
            var data = from user in db.tbl_user
                       where (user.user_email == Id && user.user_pass == Pass) || (user.user_mob_no == Id && user.user_pass == Pass)
                       select user;
            if (data.Any())
            {
                return "logIn success!!" + data.user_id;
                
            } else
            {
                return "incorrect userid and password";
            }
        }




        public decimal GetNextUser()
        {

            decimal NextUser = db.tbl_user.Max(p => p.user_id);
            return ++NextUser;
        }

        [HttpGet]
        public string UpDateUser(decimal UserId, string UserName, string UserEmail, string UserAdd, string UserLoc, string UserCity, decimal UserPin, string UserMob)
        {
            //  decimal adId = 233;
            var data = (from user in db.tbl_user
                        where user.user_id == UserId
                        select user).FirstOrDefault();
            //   data.ad_status = status;
            data.user_name = UserName;
            data.user_email = UserEmail;
            data.user_add = UserAdd;
            data.user_locality = UserLoc;
            data.user_city = UserCity;
            data.user_pincode = UserPin;
            data.user_mob_no = UserMob;
            db.SaveChanges();
            return "Record Updated";
        }
        [HttpGet]
        public string InsertUser(string UserName, string UserEmail, string UserPass, string UserAdd, string UserLoc, string UserCity, decimal UserPin, string UserMob)
        {
            decimal UserId = GetNextUser();
            tbl_user u = new tbl_user();
            u.user_id = UserId;
            u.user_name = UserName;
            u.user_email = UserEmail;
            u.user_pass = UserPass;
            u.user_add = UserAdd;
            u.user_locality = UserLoc;
            u.user_city = UserCity;
            u.user_pincode = UserPin;
            u.user_mob_no = UserMob;
            db.tbl_user.Add(u);
            db.SaveChanges();
            return "Record Saved";
        }
        [HttpGet]
        public string insertAlert(string sName, string sMob, string sEmail, string Sub, string Msg, decimal RID)
        {

            tbl_alert a = new tbl_alert();
            a.sname = sName;
            a.smob = sMob;
            a.semail = sEmail;
            a.sub = Sub;
            a.msg = Msg;
            a.rId = RID;
            db.tbl_alert.Add(a);
            db.SaveChanges();
            return "Alert Inerted";
        }


        [HttpGet]
        public List<tbl_alert> getAlert(decimal id)
        {
        var data = from al in db.tbl_alert
                   where al.rId == id
                   select al;

        List<tbl_alert> li = new List<tbl_alert>();

            foreach (var item in data)
            {
                tbl_alert a = new tbl_alert();
                a.sname = item.sname;
                a.smob =item.smob;
                a.semail =item.semail;
                a.sub =item.sub;
                a.msg =item.msg;
                a.rId =item.rId;
                
                li.Add(a);
            }
            return li;
        }


        [HttpGet]
        public string insertOTP(string id,string Otp)
        {
            tbl_forgetPass f = new tbl_forgetPass();
            f.emailOrMob = id;
            f.otp = Otp;
            db.tbl_forgetPass.Add(f);
            db.SaveChanges();
            return "OTP saved";
        }

        [HttpGet]
        public string updateOTP(string id, string Otp)
        {
            var data = (from fp in db.tbl_forgetPass
                        where fp.emailOrMob == id
                        select fp).FirstOrDefault();
            //   data.ad_status = status;
            data.otp = Otp;
           
            db.SaveChanges();
            return "OTP saved";
        }

        [HttpGet]
        public string getOTP(string id)
        {
            var data = (from fp in db.tbl_forgetPass
                       where fp.emailOrMob == id
                       select fp).FirstOrDefault();

            
            return data.otp;
        }
        public string deleteOTP(string id)
        {
            //var data = (from fp in db.tbl_forgetPass
            //            where fp.emailOrMob == id
            //            select fp).FirstOrDefault();
            //db.tbl_forgetPass.dele   

            tbl_forgetPass f = db.tbl_forgetPass.Single(asd=>asd.emailOrMob == id);
            db.tbl_forgetPass.Remove(f);
            db.SaveChanges();
            return "record deleted";
        }
        //public List<ProductListClass> GetAllUsers()
        //{
        //    var data = from product in db.tbl_productDetail
        //               join customer in db.tbl_user on product.user_id equals customer.user_id
        //               where product.ad_id == 201
        //               select new { product.pro_name, product.pro_desc,product.ad_id,product.pro_price,product.ad_status,product.pro_image,product.user_id, customer.user_name,customer.user_mob_no,customer.user_city ,customer.user_email};

        //    List<ProductListClass> li = new List<ProductListClass>();

        //    foreach (var item in data)
        //    {S
        //        ProductListClass t = new ProductListClass();
        //        t.ProductAdId = item.ad_id;
        //        t.ProductName = item.pro_name;
        //        t.ProductImage = item.pro_image;
        //        t.ProductPrice = Convert.ToDecimal( item.pro_price);
        //        t.ProductDesc = item.pro_desc;
        //        t.ProductAdStatus = item.ad_status;
        //        t.UserId = Convert.ToDecimal(item.user_id);
        //        t.UserName = item.user_name;
        //        t.UserEmail = item.user_email;
        //        t.UserCity = item.user_city;
        //        t.UserMob = item.user_mob_no;

        //        li.Add(t);
        //    }

        //    return li;
        //}

        //var data = from asd in db.tbl_user select asd;
        //var data123 = from asd in db.tbl_user select new { asd.user_email, asd.user_locality };
        //List<tbl_user> li = new List<tbl_user>();

        //List<tbl_productDetail> li1 = db.tbl_productDetail.ToList();

        //    foreach (var item in data)
        //    {
        //        tbl_user u = new tbl_user();
        //u.user_add = item.user_add;
        //        u.user_email = item.user_email;
        //        li.Add(u);
        //    }
        //    return li;

    }
}
