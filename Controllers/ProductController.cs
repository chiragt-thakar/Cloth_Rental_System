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
    public class ProductController : Controller
    {
        string constring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        // GET: Product
        [HttpGet]
        public ActionResult CreateProduct()
              {
            Product_Model model = new Product_Model();
            model.category = new Category_Model();
            model.categoryList = _Category_DropDown();
            return View(model);
        }

        

        [HttpPost]
        public ActionResult CreateProduct(Product_Model Product_Model)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Create_Product", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            HttpPostedFileBase file = Request.Files["ImageData"];
            ContentRepository service = new ContentRepository();
            Byte[] imageBytes = service.GetImageBytes(file);
            Product_Model.Image_Data = imageBytes;

            cmd.Parameters.AddWithValue("@catId", Product_Model.categoryID);
            cmd.Parameters.AddWithValue("@prdName", Product_Model.prdName);
            //cmd.Parameters.AddWithValue("@prdPrice", Product_Model.prdPrice);
            cmd.Parameters.AddWithValue("@purchasePrice", Product_Model.purchasePrice);
            cmd.Parameters.AddWithValue("@prdCode", Product_Model.prdCode);
            cmd.Parameters.AddWithValue("@rentPrice", Product_Model.rentPrice);
            cmd.Parameters.AddWithValue("@penalty", Product_Model.penalty);
            cmd.Parameters.AddWithValue("@diposit", Product_Model.diposit);
            cmd.Parameters.AddWithValue("@advanceRent", Product_Model.advanceRent);
            cmd.Parameters.AddWithValue("@imageCode",Product_Model.Image_Data );
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Manage_Product");
        }

        public ActionResult Manage_Product()
        {
            IList<Product_Model> ProductList = new List<Product_Model>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Product", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            foreach (DataRow dr in dt.Rows)
            {
                Product_Model product_Model = new Product_Model();
                product_Model.PrdId = Convert.ToInt32(dr["prdId"]);
                product_Model.catId = Convert.ToInt32(dr["catId"]);
                product_Model.prdName = Convert.ToString(dr["prdName"]);
                //product_Model.prdPrice = Convert.ToInt32(dr["prdPrice"]);
                product_Model.purchasePrice = Convert.ToInt32(dr["purchasePrice"]);
                //product_Model.productQuantity = Convert.ToInt32(dr["productQuantity"]);
                product_Model.rentPrice = Convert.ToInt32(dr["rentPrice"]);
                product_Model.penalty = Convert.ToInt32(dr["penalty"]);
                product_Model.isActive = Convert.ToInt32(dr["isActive"]);
                product_Model.Image = GetImage((byte[])(dr["imageCode"]));
                product_Model.category = new Category_Model();
                product_Model.category.catName= Convert.ToString(dr["catName"]);
                ProductList.Add(product_Model);
            }
            
            return View(ProductList);
        }

        public ActionResult Edit_product(int Id)
        {
            Product_Model obj = new Product_Model();
            obj.category = new Category_Model();
            obj.categoryList = _Category_DropDown();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Product_By_Id", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            con.Open();
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                obj.PrdId = Convert.ToInt32(dr["prdId"]);
                obj.catId = Convert.ToInt32(dr["catId"]);
                obj.rentPrice = Convert.ToInt32(dr["rentPrice"]);
                obj.purchasePrice = Convert.ToInt32(dr["purchasePrice"]);
                obj.diposit = Convert.ToInt32(dr["diposit"]);
                obj.penalty = Convert.ToInt32(dr["penalty"]);
                obj.prdCode = Convert.ToString(dr["prdCode"]);
                obj.advanceRent = Convert.ToInt32(dr["advanceRent"]);
                obj.prdName = Convert.ToString(dr["prdName"]);
               
            }
            con.Close();

            return View(obj);
        }
        [HttpPost]
        public ActionResult Edit_product(int Id, Product_Model Product_Model)
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Edit_Product", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            HttpPostedFileBase file = Request.Files["ImageData"];
            ContentRepository service = new ContentRepository();
            Byte[] imageBytes = service.GetImageBytes(file);
            Product_Model.Image_Data = imageBytes;

            cmd.Parameters.AddWithValue("@prdId", Product_Model.PrdId);
            cmd.Parameters.AddWithValue("@catId", Product_Model.categoryID);
            cmd.Parameters.AddWithValue("@prdName", Product_Model.prdName);
            //cmd.Parameters.AddWithValue("@prdPrice", Product_Model.prdPrice);
            cmd.Parameters.AddWithValue("@purchasePrice", Product_Model.purchasePrice);
            cmd.Parameters.AddWithValue("@prdCode", Product_Model.prdCode);
            cmd.Parameters.AddWithValue("@rentPrice", Product_Model.rentPrice);
            cmd.Parameters.AddWithValue("@penalty", Product_Model.penalty);
            cmd.Parameters.AddWithValue("@diposit", Product_Model.diposit);
            cmd.Parameters.AddWithValue("@advanceRent", Product_Model.advanceRent);
            cmd.Parameters.AddWithValue("@imageCode", Product_Model.Image_Data);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Manage_Product");

        }



        //public ActionResult RetrieveImage(int id)
        //{
        //    byte[] cover = GetImageFromDataBase(id);
        //    if (cover != null)
        //    {
        //        return File(cover, "image/jpg");
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //public byte[] GetImageFromDataBase(int Id)
        //{
        //    var q = ;
        //    byte[] cover = q.First();
        //    return cover;
        //}

        //private string ViewImage(byte[] arrayImage)
        //{
        //    string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
        //    ViewBag.photo = "data:image/png;base64," + base64String;
        //    return ViewBag.photo;
        //}


        public List<SelectListItem> _Category_DropDown()
        {
            List<SelectListItem> catobj = new List<SelectListItem>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Category", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sdr.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                catobj.Add(new SelectListItem()
                {
                    Text = Convert.ToString(row["catName"]),
                    Value = Convert.ToString(row["id"])
                });
            }
            return catobj;
        }
        
  

        private string GetImage(byte[] arrayImage)
        {
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;

        }
        [HttpPost]
        public JsonResult Delete_Product(string arr)
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
                    SqlCommand cmd = new SqlCommand("Sp_Delete_Product", con);
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
            }
            else
            {
                return Json(false);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Inactive_Product(string arr)
        {
            string[] arryaItem = arr.Replace("check_all,", "").Split(',');
            User_Login_Model obj = new User_Login_Model();
            obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                //for(var i=0; i<= arryaItem.Length;i++)
                {
                    SqlCommand cmd = new SqlCommand("Sp_InActive_Product", con);
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

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Active_Product(string arr)
        {
            string[] arryaItem = arr.Replace("check_all,", "").Split(',');
            User_Login_Model obj = new User_Login_Model();
            obj.xyz = arryaItem;
            if (arryaItem.Length > 0)
            {
                SqlConnection con = new SqlConnection(constring);
                foreach (var abc in arryaItem)
                {
                    SqlCommand cmd = new SqlCommand("Sp_Active_Product", con);
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

    }
}