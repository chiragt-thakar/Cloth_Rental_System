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
                return View();
                //return RedirectToAction("Manage_Customer", "Customer");
            }
            else
            {
                return View();
            }

        }
        [HttpGet]
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
                    Name = Convert.ToString(dr["cusName"]),
                    Number = Convert.ToString(dr["Number"]),
                    Address = Convert.ToString(dr["cusAddress"]),
                    isActive = Convert.ToInt32(dr["isActive"])
                });
            }
            return View(UserList);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Inactive_Customer(string arr)
        {
            try
            {
                string[] arryaItem = arr.Split(',');
                Category_Model obj = new Category_Model();
                //obj.xyz = arryaItem;
                if (arryaItem.Length > 0)
                {
                    SqlConnection con = new SqlConnection(constring);
                    foreach (var abc in arryaItem)
                    //for(var i=0; i<= arryaItem.Length;i++)
                    {
                        SqlCommand cmd = new SqlCommand("Sp_Inactive_Customer", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(abc));
                        con.Open();
                        int result = cmd.ExecuteNonQuery();
                        con.Close();
                        if (result > 0)
                        {
                            Console.WriteLine("Row afected " + result);
                        }
                        else
                        {
                            Console.WriteLine("Row ' ' afected " + result);
                        }
                    }
                    return Json(true);

                }
                else
                {
                    return Json(false);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(false);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Active_Customer(string arr)
        {
            string[] arryaItem = arr.Split(',');
            Category_Model obj = new Category_Model();
            //obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                {
                    SqlCommand cmd = new SqlCommand("Sp_Active_Customer", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(abc));
                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    con.Close();

                }
                return Json(true);
                //return RedirectToAction("Manage_User");
            }
            else
            {
                //return RedirectToAction("Admin_Dashboard");
                return Json(false);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Delete_Customer(string arr)
        {
            string[] arryaItem = arr.Split(',');
            User_Login_Model obj = new User_Login_Model();
            //obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                //for(var i=0; i<= arryaItem.Length;i++)
                {
                    SqlCommand cmd = new SqlCommand("Sp_Delete_Customer", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(abc));
                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.WriteLine("Data   deleted sucessfully");
                    }
                    else
                    {
                        Console.WriteLine("Data not deleted sucessfully");

                    }
                    con.Close();
                }
                return Json(true);
                //return RedirectToAction("Manage_User");
            }
            else
            {
                return Json(false);
                //return RedirectToAction("Admin_Dashboard");
            }
        }
    }
    }
