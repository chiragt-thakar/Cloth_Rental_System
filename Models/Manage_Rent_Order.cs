using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cloth_Rental_System.Models
{
    public class Manage_Rent_Order
    {
		public int orderID { get; set; }
		public int customerId { get; set; }
		public string customerName { get; set; }
		
		public int totalRent { get; set; }
		public int totalDeposit { get; set; }
		public int totalAdvanceRent { get; set; }
		public DateTime deliveryDate { get; set; }
		public DateTime returnDate { get; set; }
		public List<Product_Model> productList { get; set; }






		
	}
}