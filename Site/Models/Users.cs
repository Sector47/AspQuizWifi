using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/**
 * A MODEL OF USER DB DATA, USED FOR FORM VALIDATION
 *
 **/

namespace Site.Models
{

    public class Users
    {

        public int UserID { get; set; }


        // Data annotation for required and all letters regex
        [Required(ErrorMessage = "Please enter the first name.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter only letters in the first name.")]
        public string FirstName { get;set; }

        // Data annotaiton for required and all leters (with apostrphew, hyphen, spaces allowed) regex
        [Required(ErrorMessage = "Please enter the last name.")]
        [RegularExpression("^[a-zA-Z ]*[-a-zA-Z]*['a-zA-Z]*[\\\\sa-zA-Z]*[a-zA-Z]$", ErrorMessage = "Please enter only letters, hyphens, apostrophes, or spaces in the last name.")]
        public string LastName { get; set; }

        // Data annotation for required and all letters regex
        [Required(ErrorMessage = "Please enter a username.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Please enter only letters in the first name.")]
        public string Username { get; set; }

        // Data annotation for required password
        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }

        // IsInstructor is boolean, so checkbox is optional
        public bool IsInstructor { get; set; }
    }
}