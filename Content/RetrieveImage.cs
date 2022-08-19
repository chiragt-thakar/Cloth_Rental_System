//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Cloth_Rental_System.Content
//{
//    public class RetrieveImage
//    {
//        public ActionResult RetrieveImage(int id)
//        {
//            byte[] cover = GetImageFromDataBase(id);
//            if (cover != null)
//            {
//                return File(cover, "image/jpg");
//            }
//            else
//            {
//                return null;
//            }
//        }
//        public byte[] GetImageFromDataBase(int Id)
//        {
//            var q = from temp in db.Contents where temp.ID == Id select temp.Image;
//            byte[] cover = q.First();
//            return cover;
//        }
//    }
//}