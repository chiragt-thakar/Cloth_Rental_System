using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using System.Web;

namespace Cloth_Rental_System.Models
{
    public class Category_Model
    {
        public int? id{get;set;}
        public string catName { get; set; }
        public int isActive { get; set; }
        public string gender { get; set; }

    }
}