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
using System.Collections;
using System.Xml;
using System.Globalization;

namespace Cloth_Rental_System.Controllers
{
    public class Product_OrderController : Controller
    {
        
        string constring = ConfigurationManager.ConnectionStrings["connstring"].ConnectionString;

        // GET: Order
        [HandleError] 
        public ActionResult Rent_Product()
        {
            Order_Product_Model order_Product_Model = new Order_Product_Model();
            order_Product_Model.Product = new Product_Model();
            order_Product_Model.Product.productList = _Product_Dropdown();
            order_Product_Model.Product.userList = _User_Dropdown();
            return View(order_Product_Model);
        }

        [HttpPost]
        [HandleError]
        public  JsonResult Rent_Product(Order_Product_Model order_Product_Model )
        {
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Create_Rent";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            con.Open();
          
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custId", order_Product_Model.Customer_Id);
            cmd.Parameters.AddWithValue("@totalAmount", order_Product_Model.Total_Rent);
            cmd.Parameters.AddWithValue("@totalDiposite", order_Product_Model.Total_Diposit);
            cmd.Parameters.AddWithValue("@totalAdvanceRent", order_Product_Model.Total_Advance_Rent);
            cmd.Parameters.AddWithValue("@orderDate", order_Product_Model.Ord_Date);
            cmd.Parameters.AddWithValue("@returnDate", order_Product_Model.Return_Date);
            int a = cmd.ExecuteNonQuery();

            cmd.CommandText = "select MAX(rentId) from tblRent";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            object result = cmd.ExecuteScalar();
           
            order_Product_Model.rentId = Convert.ToInt32(result);
            int res = Convert.ToInt32(result);
            //create_rent_detail(res);

            foreach (var price in order_Product_Model.productList)
            {
                SqlCommand cmd1 = new SqlCommand();

                cmd1.CommandText = "Sp_Create_Rent_Detail";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Connection = con;

                cmd1.Parameters.AddWithValue("@prdId", price.PrdId); 
                cmd1.Parameters.AddWithValue("@rentId", res);//common rent id 
                cmd1.Parameters.AddWithValue("@rentPrice", price.rentPrice);
                cmd1.Parameters.AddWithValue("@advanceRent", price.advanceRent);
                cmd1.Parameters.AddWithValue("@diposit", price.diposit);
              int ab =  cmd1.ExecuteNonQuery();
            }
            con.Close();
            if (a >= 1)
            {
            return Json(true);
            }
            return Json(false);
        }
       
        [HttpPost]
        [HandleError]
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
        [HandleError]
        public List<SelectListItem> _User_Dropdown()
        {
            List<SelectListItem> Customer_list = new List<SelectListItem>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Customer", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sdr.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = Convert.ToString(row["cusName"]);
                obj.Value = Convert.ToString(row["cusId"]);
                Customer_list.Add(obj);
            }
            return Customer_list;
        }

        [HandleError]
        public List<SelectListItem> _Product_Dropdown()
        {
            List<SelectListItem> prdobj = new List<SelectListItem>();
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Dd_Select_Product", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                prdobj.Add(new SelectListItem()
                {
                    Text = Convert.ToString(row["prdName"]),
                    Value = Convert.ToString(row["PrdId"])
                });
            }
            return prdobj;
        }
        public List<SelectListItem> _User_Dropdown2(int? id) //use in edit_rent_order
        {
            List<SelectListItem> Customer_list = new List<SelectListItem>();
            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Select_Customer_By_Id", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            con.Open();
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                SelectListItem obj = new SelectListItem();
                obj.Text = Convert.ToString(row["cusName"]);
                obj.Value = Convert.ToString(row["cusId"]);
                Customer_list.Add(obj);
            }
            con.Close();
            return Customer_list;
        }
       
        
        public List<SelectListItem> _Product_Dropdown2(int? id)  //used in edit_rent_order
        {
            List<SelectListItem> prdobj = new List<SelectListItem>();
            SqlConnection conn = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand("Sp_Dd_Select_Product_By_Id", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                prdobj.Add(new SelectListItem()
                {
                    Text = Convert.ToString(row["prdName"]),
                    Value = Convert.ToString(row["PrdId"])
                });
            }
            conn.Close();
            return prdobj;
        }
        [HandleError]
        public ActionResult _rent_Order(int chnId)
        {
            Product_Model product_Model = new Product_Model();
            if (chnId >= 1)
            {  
                product_Model.virtualPrdId = chnId + 1;
            }
            product_Model.productList = _Product_Dropdown();
            return PartialView(product_Model);
        }
      public ActionResult manage_rent_order()
        {
            IList<Manage_Rent_Order> list_obj = new List<Manage_Rent_Order>();
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_List_User_Rent";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            DataSet ds = new DataSet();
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            sd1.Fill(ds);
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            con.Close();
            dt1 = ds.Tables[0]; 
            dt2 = ds.Tables[1];
            foreach (DataRow sdr in dt1.Rows)
            {
                Manage_Rent_Order Manage_Rent_Order = new Manage_Rent_Order();
                Manage_Rent_Order.orderID = Convert.ToInt32(sdr["rentId"]);
                ViewBag.rentId= Convert.ToInt32(sdr["rentId"]);
                Manage_Rent_Order.customerId = Convert.ToInt32(sdr["custId"]);
                Manage_Rent_Order.totalRent = Convert.ToInt32(sdr["totalAmount"]);
                Manage_Rent_Order.totalDeposit = Convert.ToInt32(sdr["totalDiposite"]);
                Manage_Rent_Order.totalAdvanceRent = Convert.ToInt32(sdr["totalAdvanceRent"]);
                Manage_Rent_Order.customerName = Convert.ToString(sdr["cusName"]);
                Manage_Rent_Order.deliveryDate = ToRfc3339String((DateTime)sdr["orderDate"]);
                Manage_Rent_Order.returnDate = ToRfc3339String((DateTime)(sdr["returnDate"]));




                DataView dv = new DataView(dt2);
                string filter = String.Format("rentId = '{0}'", sdr[0].ToString());
                dv.RowFilter = filter;
                //dv.RowFilter = "rentId = (sdr['rentId']).ToString()";


                //DataRow[] sorted_Rows;
                //sorted_Rows = dt2.Select("rentId = Manage_Rent_Order.orderID");



                List<Product_Model> productList = new List<Product_Model>();
                foreach (DataRowView sdr1 in dv)
                {
                    Product_Model product_Model = new Product_Model();
                    product_Model.PrdId = Convert.ToInt32(sdr1["prdId"]);
                    product_Model.prdName = Convert.ToString(sdr1["prdName"]);
                    product_Model.Image = GetImage((byte[])(sdr1["imageCode"]));
                    productList.Add(product_Model);
                }
                Manage_Rent_Order.productList = productList;
                list_obj.Add(Manage_Rent_Order);
            }

           
            return View(list_obj);
            //    foreach (DataRow sdr in dt.Rows)
            //{
            //    foreach (DataRow sdr in dt.Rows)
            //{
            //    Order_Product_Model Order_Product_Model = new Order_Product_Model();

            //    DataColumnCollection columns = dt.Columns;
            //    int RENTID = Convert.ToInt32(sdr["rentId"]);
            //    if (columns.Contains("rentId"))
            //    {
            //        con.Open();
            //        SqlCommand cmd2 = new SqlCommand("Sp_select_Product_using_rentId",con);
            //        cmd2.CommandType = CommandType.StoredProcedure;
            //        cmd2.Parameters.AddWithValue("@rentId", RENTID);
            //        SqlDataAdapter sd2 = new SqlDataAdapter(cmd2);
            //        DataTable dt2 = new DataTable();
            //        sd2.Fill(dt2);
            //        Order_Product_Model.productList2 = new List<Order_Product_Model>();
            //        foreach (DataRow sdr2 in dt2.Rows)
            //        {
            //            Console.WriteLine(sdr2);
            //            Order_Product_Model.Product_Name = Convert.ToString(sdr2["prdName"]);
            //            Order_Product_Model.productList2.Add(Order_Product_Model);
            //        }
            //        con.Close();
            //    }
            //    Order_Product_Model.Customer_Name = Convert.ToString(sdr["cusName"]);
            //    Order_Product_Model.Customer_Id = Convert.ToInt32(sdr["custId"]);
            //    Order_Product_Model.Ord_Date = Convert.ToDateTime(sdr["orderDate"]);
            //    Order_Product_Model.Return_Date = Convert.ToDateTime(sdr["returnDate"]);
            //    Order_Product_Model.Total_Rent = Convert.ToInt32(sdr["totalAmount"]);
            //    Order_Product_Model.Total_Advance_Rent = Convert.ToInt32(sdr["totalAdvanceRent"]);
            //    Order_Product_Model.Total_Diposit = Convert.ToInt32(sdr["totalDiposite"]);
            //    list_obj.Add(Order_Product_Model);
            //}

        }

        

        private string GetImage(byte[] arrayImage)
        {
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;

        }

        public ActionResult Edit_Rent_Order(int id)
        {

            IList<Manage_Rent_Order> list_obj = new List<Manage_Rent_Order>();
            SqlConnection con = new SqlConnection(constring);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_select_rent_list_by_Id";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Connection = con;
            DataSet ds = new DataSet();
            SqlDataAdapter sd1 = new SqlDataAdapter(cmd);
            sd1.Fill(ds);
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            con.Close();
            dt1 = ds.Tables[0];
            dt2 = ds.Tables[1];
            foreach (DataRow sdr in dt1.Rows)
            {
                Manage_Rent_Order Manage_Rent_Order = new Manage_Rent_Order();
                Manage_Rent_Order.orderID = Convert.ToInt32(sdr["rentId"]);
                ViewBag.rentId = Convert.ToInt32(sdr["rentId"]);
                Manage_Rent_Order.customerId = Convert.ToInt32(sdr["custId"]);
                Manage_Rent_Order.totalRent = Convert.ToInt32(sdr["totalAmount"]);
                Manage_Rent_Order.totalDeposit = Convert.ToInt32(sdr["totalDiposite"]);
                Manage_Rent_Order.totalAdvanceRent = Convert.ToInt32(sdr["totalAdvanceRent"]);
                Manage_Rent_Order.customerName = Convert.ToString(sdr["cusName"]);
                Manage_Rent_Order.deliveryDate = ToRfc3339String((DateTime)(sdr["orderDate"]));
                Manage_Rent_Order.returnDate = ToRfc3339String((DateTime)(sdr["returnDate"]));
                Manage_Rent_Order.userList = _User_Dropdown2(Manage_Rent_Order.customerId);



                DataView dv = new DataView(dt2);
                string filter = String.Format("rentId = '{0}'", sdr[0].ToString());
                dv.RowFilter = filter;
                //dv.RowFilter = "rentId = (sdr['rentId']).ToString()";


                //DataRow[] sorted_Rows;
                //sorted_Rows = dt2.Select("rentId = Manage_Rent_Order.orderID");



                List<Product_Model> productList = new List<Product_Model>();
                foreach (DataRowView sdr1 in dv)
                {
                    Product_Model product_Model = new Product_Model();
                    product_Model.PrdId = Convert.ToInt32(sdr1["prdId"]);
                    product_Model.rentdetailId = Convert.ToInt32(sdr1["rentDetailId"]);
                    product_Model.rentPrice = Convert.ToInt32(sdr1["rentPrice"]);
                    product_Model.advanceRent = Convert.ToInt32(sdr1["advanceRent"]);
                    product_Model.diposit = Convert.ToInt32(sdr1["diposit"]);
                    product_Model.prdName = Convert.ToString(sdr1["prdName"]);
                    product_Model.Image = GetImage((byte[])(sdr1["imageCode"]));
                    product_Model.productList = _Product_Dropdown();
                    productList.Add(product_Model);
                }
                Manage_Rent_Order.productList = productList;
                list_obj.Add(Manage_Rent_Order); 
            }


            return View(list_obj);
        }
        [HttpPost]
        [HandleError]
        public JsonResult Edit_Rent_Order(Order_Product_Model order_Product_Model)
        {

            SqlConnection con = new SqlConnection(constring);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Sp_Create_Rent_By_Id";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            con.Open();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custId", order_Product_Model.Customer_Id);
            cmd.Parameters.AddWithValue("@rentId", order_Product_Model.rentId);
            cmd.Parameters.AddWithValue("@totalAmount", order_Product_Model.Total_Rent);
            cmd.Parameters.AddWithValue("@totalDiposite", order_Product_Model.Total_Diposit);
            cmd.Parameters.AddWithValue("@totalAdvanceRent", order_Product_Model.Total_Advance_Rent);
            cmd.Parameters.AddWithValue("@orderDate", order_Product_Model.Ord_Date);
            cmd.Parameters.AddWithValue("@returnDate", order_Product_Model.Return_Date);
            int a = cmd.ExecuteNonQuery();

            //cmd.CommandText = "select MAX(rentId) from tblRent";
            //cmd.CommandType = CommandType.Text;
            //cmd.Connection = con;
            //object result = cmd.ExecuteScalar();

            //order_Product_Model.rentId = Convert.ToInt32(result);
            //int res = Convert.ToInt32(result);
            //create_rent_detail(res);

            foreach (var price in order_Product_Model.productList)
            {
                SqlCommand cmd1 = new SqlCommand();

                cmd1.CommandText = "Sp_Create_Rent_Detail_By_Id";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Connection = con;

                cmd1.Parameters.AddWithValue("@prdId", price.PrdId);
                cmd1.Parameters.AddWithValue("@rentdetailId", price.rentdetailId);
                cmd1.Parameters.AddWithValue("@rentId", order_Product_Model.rentId);//common rent id 
                cmd1.Parameters.AddWithValue("@rentPrice", price.rentPrice);
                cmd1.Parameters.AddWithValue("@advanceRent", price.advanceRent);
                cmd1.Parameters.AddWithValue("@diposit", price.diposit);
                int ab = cmd1.ExecuteNonQuery();
            }
            con.Close();
            if (a >= 1)
            {
                return Json(true);
            }
            return Json(false);
        }
        public string ToRfc3339String(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
        }

        private string GetDate(DateTime DateTime)
        {
            DateTime UtcDateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime);
            return XmlConvert.ToString(UtcDateTime, XmlDateTimeSerializationMode.Utc);
        }
        //public ActionResult _Product_List()
        //{
        //    IList<Order_Product_Model> list_obj = new List<Order_Product_Model>();

        //    SqlConnection con = new SqlConnection(constring);
        //    SqlCommand cmd1 = new SqlCommand();
        //    cmd1.CommandText = "select * from tblRent";
        //    cmd1.CommandType = CommandType.Text;
        //    cmd1.Connection = con;
        //    SqlDataAdapter sd1 = new SqlDataAdapter(cmd1);
        //    DataTable dt1 = new DataTable();
        //    //DataColumnCollection columns = dt1.Columns;
        //    //if (columns.Contains("rentId"))
        //    //{

        //    //}
        //    con.Open();
        //    sd1.Fill(dt1);
        //    con.Close();
        //    foreach (DataRow dr in dt1.Rows)
        //    {
        //        Order_Product_Model Order_Product_Model = new Order_Product_Model();
        //        //Order_Product_Model.prdName = Convert.ToString(dr["cusName"]);
        //        Order_Product_Model.rentDetailId = Convert.ToInt32(dr["rentDetailId"]);
        //        Order_Product_Model.rentId = Convert.ToInt32(dr["rentId"]);
        //        Order_Product_Model.rentPrice = Convert.ToInt32(dr["rentPrice"]);
        //        Order_Product_Model.advanceRent = Convert.ToInt32(dr["advanceRent"]);
        //        Order_Product_Model.diposit = Convert.ToInt32(dr["diposit"]);
        //        Order_Product_Model.productList2.Add(Order_Product_Model);
        //    }
        //    return View(list_obj);
        //}

    }
}