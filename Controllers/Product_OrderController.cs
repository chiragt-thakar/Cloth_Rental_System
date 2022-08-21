using Cloth_Rental_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Cloth_Rental_System.Controllers
{
    public class Product_OrderController : Controller
    {
        int idvala = 1;
        string constring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        // GET: Order
        public ActionResult Rent_Product()
        {
            Order_Product_Model order_Product_Model = new Order_Product_Model();
            order_Product_Model.Product = new Product_Model();
            order_Product_Model.Product.productList = _Product_Dropdown();
            return View(order_Product_Model);
        }
        [HttpPost]
        public JsonResult Product_data(int ProductId)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Product_By_Id", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", ProductId);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            Product_Model productModel = new Product_Model();
            foreach (DataRow dr in dt.Rows)
            {
                //product_Model.Product.prdName = Convert.ToString(dr["prdName"]);
                productModel.rentPrice = Convert.ToInt32(dr["rentPrice"]);
                productModel.diposit = Convert.ToInt32(dr["diposit"]);
                productModel.advanceRent = Convert.ToInt32(dr["advanceRent"]);
            }
            return Json(productModel);
        }


        [HttpPost]
        public ActionResult Rent_Product(Order_Product_Model pobj)
        {

            return View();
        }


        public ActionResult _User_Dropdown()
        {
            List<Customer_Model> Customer_list = new List<Customer_Model>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Customer", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sdr.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                Customer_Model obj = new Customer_Model();
                obj.Name = Convert.ToString(row["cusName"]);
                obj.cusId = Convert.ToInt32(row["cusId"]);
                Customer_list.Add(obj);
            }
            return PartialView(Customer_list);
        }

        public List<SelectListItem> _Product_Dropdown()
        {
            List<SelectListItem> prdobj = new List<SelectListItem>();
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Dd_Select_Product",conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            foreach(DataRow row in dt.Rows)
            {
                prdobj.Add(new SelectListItem()
                {
                    Text = Convert.ToString(row["prdName"]),
                    Value = Convert.ToString(row["PrdId"])
                });
            }
            return prdobj;
        } 
        public ActionResult _Product_Dropdown1()
        {
            List<Product_Model> prdobj = new List<Product_Model>();
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Dd_Select_Product",conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            foreach(DataRow row in dt.Rows)
            {
                prdobj.Add(new Product_Model()
                {
                    prdName = row["prdName"].ToString(),
                    PrdId = Convert.ToInt32(row["PrdId"])
                });
            }
            return PartialView(prdobj);
        }
        public ActionResult _rent_Order()
        {
           
            return PartialView();
        }

        public  JsonResult inc_count()
        {
            int ac = idvala++;
            return Json(ac);
        }
    }
}