using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloth_Rental_System.Models
{
    public class Order_Product_Model
    {
        public Customer_Model customer { get; set; }
        public int rentPrice { get; set; }
        public int advanceRent { get; set; }
        public int diposit { get; set; }
        public int rentDetailId { get; set; }
        public int rentId { get; set; }
        public int Customer_Id { get; set; }
        public int Total_Rent { get; set; }
        public int Total_Advance_Rent { get; set; }
        public int Total_Diposit { get; set; }
        public string Customer_Name { get; set; }
        public string Product_Name { get; set; }
        [ DataType(DataType.Date)]
        public DateTime Ord_Date { get; set; }
      
        public DateTime Return_Date { get; set; }
        
        
        public Product_Model Product { get; set; }
        public List<Product_Model> productList { get; set; }
        public List<Order_Product_Model> productList2 { get; set; }

    }
}