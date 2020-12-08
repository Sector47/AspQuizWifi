using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Site.Models
{

    public class Users
    {

        public int UserID { get; set; }

        [Required(ErrorMessage = "Please enter the first name.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter only letters in the first name.")]
        public string FirstName { get;set; }

        [Required(ErrorMessage = "Please enter the last name.")]
        [RegularExpression("^[a-zA-Z ]*[-a-zA-Z]*['a-zA-Z]*[\\\\sa-zA-Z]*[a-zA-Z]$", ErrorMessage = "Please enter only letters, hyphens, apostrophes, or spaces in the last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a username.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter only letters in the first name.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }

        public bool IsInstructor { get; set; }
    }
}