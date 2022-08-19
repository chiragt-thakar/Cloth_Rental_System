using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cloth_Rental_System.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace Cloth_Rental_System.Controllers
{
    public class Image_UploadController : Controller
    {
        // GET: Image_Upload
     
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(File_Upload file)
        {
            try
            {

                Byte[] bytes = null;
                if (file.Filepic.FileName != null)
                {
                    Stream fs = file.Filepic.InputStream;
                    BinaryReader br = new BinaryReader(fs);
                    bytes = br.ReadBytes((Int32)fs.Length);
                    string connectionstring = Convert.ToString(ConfigurationManager.ConnectionStrings["connstring"]);
                    SqlConnection con = new SqlConnection(connectionstring);
                    SqlCommand cmd = new SqlCommand("Sp_Upload_Image", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FileNames", file.Name);
                    cmd.Parameters.AddWithValue("@Filepic", bytes);
                    cmd.Parameters.AddWithValue("@UploadDate", DateTime.Now);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    if(i == 1)
                    {
                        Console.WriteLine("hello baby");
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                    con.Close();
                    ViewBag.Image = ViewImage(bytes);

                }

            }

            catch (Exception)

            {
                throw;
            }

            return View();
        }
        private string ViewImage(byte[] arrayImage)
        {
            string base64String = Convert.ToBase64String(arrayImage, 0, arrayImage.Length);
            return "data:image/png;base64," + base64String;
        }
    }
}