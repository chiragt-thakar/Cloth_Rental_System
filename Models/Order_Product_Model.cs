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
        public User_Login_Model User_Name { get; set; }
        public string Product_Name { get; set; }
        [ DataType(DataType.Date)]
        public DateTime Ord_Date { get; set; }
        [ DataType(DataType.Time)]
        public DateTime Ord_Time { get; set; } 
        [ DataType(DataType.Date)]
        public DateTime Return_Date { get; set; }
        [ DataType(DataType.Time)]
        public DateTime Return_Time { get; set; }
        public Product_Model Product { get; set; }
        
    }
}