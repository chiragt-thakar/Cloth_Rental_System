using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
		public string deliveryDate { get; set; }
		public string returnDate { get; set; }
		public Product_Model Product { get; set; }
		public List<Product_Model> productList { get; set; }
		public List<SelectListItem> userList { get; set; }
		public List<SelectListItem> productList_dr { get; set; }
		public List<SelectListItem> categoryList { get; set; }







	}
}