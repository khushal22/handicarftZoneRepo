using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ProductListClass
    {
        public decimal ProductAdId { get; set; }
        public decimal ProductCatId { get; set; }
        public decimal ProductSubCatId { get; set; }
        public decimal UserId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDesc { get; set; }
        public string ProductAdStatus { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserCity { get; set; }
        public string UserMob { get; set; }
    }
}