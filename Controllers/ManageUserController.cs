using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.MyDB;
using WebApplication1.Models;
using System.Net.Mail;

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
        public Tuple<string,tbl_user> login(string Id, string Pass)
        {
            var data = from user in db.tbl_user
                       where (user.user_email == Id && user.user_pass == Pass) || (user.user_mob_no == Id && user.user_pass == Pass)
                       select user;
            tbl_user u = new tbl_user();
            if (data.Any())
            {
               
                foreach (var item in data)
                 {
                    u.user_id = item.user_id;
                    u.user_name = item.user_name;
                    u.user_email = item.user_email;
                    u.user_add = item.user_add;
                    u.user_locality = item.user_locality;
                    u.user_city = item.user_city;
                    u.user_pincode = item.user_pincode;
                    u.user_mob_no = item.user_mob_no;
               }

                return Tuple.Create("Logged In!!", u);

            } else
            {
                return Tuple.Create("incorrect userid and password",u);
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
        public Tuple< string,tbl_user> InsertUser(string UserName, string UserEmail, string UserPass, string UserAdd, string UserLoc, string UserCity, decimal UserPin, string UserMob)
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

            tbl_user s = new tbl_user();
            s.user_id = UserId;
            s.user_name = UserName;
            s.user_email = UserEmail;
            s.user_add = UserAdd;
            s.user_locality = UserLoc;
            s.user_city = UserCity;
            s.user_pincode = UserPin;
            s.user_mob_no = UserMob;
            return Tuple.Create("Record Saved ",s);
        }
        [HttpGet]
        public string insertAlert(string sName, string sMob, string sEmail, string Sub, string Msg, decimal RID,decimal SID)
        {

            tbl_alert a = new tbl_alert();
            a.sname = sName;
            a.smob = sMob;
            a.semail = sEmail;
            a.sub = Sub;
            a.msg = Msg;
            a.rId = RID;
            a.sid = SID;
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
                a.sid = item.sid;
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
        

            tbl_forgetPass f = db.tbl_forgetPass.Single(asd=>asd.emailOrMob == id);
            db.tbl_forgetPass.Remove(f);
            db.SaveChanges();
            return "record deleted";
        }
        [HttpGet]
        public string SendMail( string ReceiverEmailID, string Subject, string MailBody)
        {
            var data = (from fp in db.tbl_emailDetail
                        where fp.id == 1
                        select fp).FirstOrDefault();

           
            string SenderEmailID,  SenderPassword;
            SenderEmailID = data.emailId;
            SenderPassword = data.pass;
            MailMessage mail = new MailMessage();
            mail.To.Add(ReceiverEmailID);
            mail.From = new MailAddress(SenderEmailID);
            mail.Subject = Subject;
            mail.Body = MailBody;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(SenderEmailID, SenderPassword);
            smtp.EnableSsl = true;
            string IsSent = "";
            try
            {
                smtp.Send(mail);
                IsSent = "Mail Sent Successfully";
            }
            catch (Exception e)
            {
                IsSent = e.Message;
            }


              return IsSent;
           
        }
    }
}
