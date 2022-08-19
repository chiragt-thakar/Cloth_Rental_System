using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cloth_Rental_System.Models
{
    public class User_Login_Model
    {

        public int User_Id { get; set; }

        //[Required(ErrorMessage = "Field can't be empty")]
        public string User_Name { get; set; }

        //[Required]
        //[EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        public string User_Email { get; set; }

        //[Required(ErrorMessage = "Field can't be empty")]
        public string User_Password { get; set; }

        //[Required(ErrorMessage = "Field can't be empty")]
        public string User_Type { get; set; }
        public int Status { get; set; }
        public string[] xyz { get; set; }
    }
}