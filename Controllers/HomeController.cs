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
    public class HomeController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User_Login_Model mobj)
        {
           
                SqlConnection con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("Sp_User_Login", con);
                con.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", mobj.User_Email);
                cmd.Parameters.AddWithValue("@Password", mobj.User_Password);

                SqlDataReader sdr = cmd.ExecuteReader();
           

            if (sdr.Read())
            {
                mobj.User_Name = sdr["User_Name"].ToString();
                mobj.User_Type = sdr["User_Type"].ToString();
                mobj.User_Id = Convert.ToInt32(sdr["User_Id"]);
                mobj.Status = Convert.ToInt32(sdr["Status"]);
                Session["Email"] = mobj.User_Email.ToString();
                Session["Name"] = mobj.User_Name.ToString();
                Session["Type"] = mobj.User_Type.ToString();
                //return RedirectToAction("Admin_Dashboard");
                if (mobj.User_Type == "ADMIN" && mobj.Status == 1)
                {
                    return RedirectToAction("Admin_Dashboard");
                }
                else
                {
                    return RedirectToAction("User_Home");
                }
            }
            else
            {
                ViewBag.Message = "User Login Details Failed!!";
            }
            con.Close();
            return View();
        }
        public ActionResult Admin_Dashboard()
        {
            if (Session["Type"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index");
            }
        }
        //Create user 
        public ActionResult Create_user()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Create_user(User_Login_Model objm)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("Sp_Create_User", con);
                con.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", objm.User_Name);
                cmd.Parameters.AddWithValue("@Email", objm.User_Email);
                cmd.Parameters.AddWithValue("@Password", objm.User_Password);
                cmd.Parameters.AddWithValue("@User_Type", objm.User_Type);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Manage_User");
            }
            else
            {
                return View();
            }
        }
        //Manage User
        public ActionResult Manage_User()
        {
            IList<User_Login_Model> UserList = new List<User_Login_Model>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_User", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                UserList.Add(new User_Login_Model
                {
                    User_Id = Convert.ToInt32(dr["User_Id"]),
                    User_Name = Convert.ToString(dr["User_Name"]),
                    User_Email = Convert.ToString(dr["User_Email"]),
                    User_Password = Convert.ToString(dr["User_Password"]),
                    User_Type = Convert.ToString(dr["User_Type"]),
                    Status = Convert.ToInt32(dr["Status"])

                });
            }
            return View(UserList);
        }
        //logout user
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Delete_user(string arr)
        {
            string[] arryaItem = arr.Split(',');
            User_Login_Model obj = new User_Login_Model();
            obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                //for(var i=0; i<= arryaItem.Length;i++)
                {
                    SqlCommand cmd = new SqlCommand("Sp_Delete_User", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(abc));
                    con.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.WriteLine("Data deleted sucessfully");
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
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Inactive_user(string arr)
        {
            string[] arryaItem = arr.Split(',');
            User_Login_Model obj = new User_Login_Model();
            obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                //for(var i=0; i<= arryaItem.Length;i++)
                {
                    SqlCommand cmd = new SqlCommand("Sp_Inactive_User", con);
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
                //return RedirectToAction("Manage_User");
            }
            else
            {
                return Json(false);
                //return RedirectToAction("Admin_Dashboard");
            }
        }

        //Active User
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Active_User(string arr)
        {
            string[] arryaItem = arr.Split(',');
            User_Login_Model obj = new User_Login_Model();
            obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                {
                    SqlCommand cmd = new SqlCommand("Sp_Active_User", con);
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
        public ActionResult User_Home()
        {
            return View();
        }
        public ActionResult Category()
        {
            return View();
        }
        public ActionResult Edit_User(int id)
        {
            User_Login_Model obj = new User_Login_Model();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_User_By_Id", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                obj.User_Id = Convert.ToInt32(dr["User_Id"]);
                obj.User_Name = Convert.ToString(dr["User_Name"]);
                obj.User_Email = Convert.ToString(dr["User_Email"]);
                obj.User_Password = Convert.ToString(dr["User_Password"]);
                obj.User_Type = Convert.ToString(dr["User_Type"]);
                obj.Status = Convert.ToInt32(dr["Status"]);
            }
            con.Close();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit_User(int id, User_Login_Model obj)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Edit_User", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", obj.User_Name);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Email", obj.User_Email);
            cmd.Parameters.AddWithValue("@Password", obj.User_Password);
            cmd.Parameters.AddWithValue("@Type", obj.User_Type);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Edit_User");
        }
        [HttpPost]
        public ActionResult Category(Category_Model objm)
        {
            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(constring);
                SqlCommand cmd = new SqlCommand("Sp_Create_Category", con);
                con.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", objm.catName);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                cmd.ExecuteNonQuery();
                con.Close();
                return RedirectToAction("Manage_Category");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Manage_Category()
        {
            IList<Category_Model> CatList = new List<Category_Model>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Category", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                CatList.Add(new Category_Model
                {
                    id = Convert.ToInt32(dr["id"]),
                    catName = Convert.ToString(dr["catName"]),
                    isActive = Convert.ToInt32(dr["isActive"])
                });
            }
            return View(CatList);
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Inactive_Category(string arr)
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
                        SqlCommand cmd = new SqlCommand("Sp_Inactive_Category", con);
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
        public JsonResult Active_Category(string arr)
        {
            string[] arryaItem = arr.Split(',');
            Category_Model obj = new Category_Model();
            //obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                {
                    SqlCommand cmd = new SqlCommand("Sp_Active_Category", con);
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
        public JsonResult Delete_Category(string arr)
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
                    SqlCommand cmd = new SqlCommand("Sp_Delete_Category", con);
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

        public ActionResult Edit_Category(int id)
        {
            Category_Model obj = new Category_Model();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Category_By_Id", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                obj.id = Convert.ToInt32(dr["id"]);
                obj.catName = Convert.ToString(dr["catName"]);
            }
            con.Close();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit_category(int id, Category_Model obj)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Edit_Category", con);
            con.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", obj.catName);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
            con.Close();
            return View();
        }
    }
}