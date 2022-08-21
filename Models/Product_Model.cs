using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Cloth_Rental_System.Models
{
    public class Product_Model
    { 
        public int? PrdId { get; set; }
        public int? catId { get; set; }
        //public IEnumerable<SelectListItem> catName { get; set; }
        public string catName { get; set; }
        public string prdName { get; set; } 
        public int? prdPrice { get; set; } 
        public int? purchasePrice { get; set; } 
        public int? rentPrice { get; set; } 
        public int? penalty { get; set; } 
        public int? diposit { get; set; } 
        public string imageName  { get; set; } 
        public string Image  { get; set; } 
        public HttpPostedFileBase imageCode { get; set; } 
        public byte[] bytes { get; set; }
        public byte[] Image_Data { get; set; }
        public int? categoryID { get; set; }
        public int? isActive { get; set; }
        public int? advanceRent { get; set; }
        public int? prdCode { get; set; }
        public Category_Model category { get; set; }
        //public List<SelectListItem> categoryList { get; set; }
        public List<SelectListItem> productList { get; set; }
    }

}