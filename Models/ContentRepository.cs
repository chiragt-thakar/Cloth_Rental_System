using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Cloth_Rental_System.Models
{
    public class ContentRepository
    {
      
        public byte[] GetImageBytes(HttpPostedFileBase file)
        {
           return  ConvertToBytes(file);           
        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }

        //Get Image


    }
}