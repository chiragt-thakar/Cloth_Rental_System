using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloth_Rental_System.Models
{
    public class Customer_Model
    {
        public int cusId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }
        public int isActive { get; set; } 
    }
}