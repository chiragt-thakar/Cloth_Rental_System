using Cloth_Rental_System.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cloth_Rental_System.Controllers
{
    public class CustomerController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        // GET: Customer
        public ActionResult Create_customer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create_customer(Customer_Model cmodel)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("Sp_Create_Customer", con);
                con.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", cmodel.Name);
                cmd.Parameters.AddWithValue("@Number", cmodel.Number);
                cmd.Parameters.AddWithValue("@Address", cmodel.Address);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Manage_Customer");
            }
            else
            {
                return View();
            }

        }

        public ActionResult Manage_Customer()
        {
            IList<Customer_Model> UserList = new List<Customer_Model>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Customer", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                UserList.Add(new Customer_Model
                {
                    cusId = Convert.ToInt32(dr["cusId"]),
                    Name = Convert.ToString(dr["Name"]),
                    Number = Convert.ToString(dr["Number"]),
                    Address = Convert.ToString(dr["Address"]),
                    isActive = Convert.ToInt32(dr["isActive"])
                });
            }
            return View(UserList);
        }
    }
    }
