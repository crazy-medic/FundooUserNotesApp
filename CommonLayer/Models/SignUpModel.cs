using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Enter your name")]
        [RegularExpression("^[a-zA-Z]{3,}", ErrorMessage = "First Name should contain minimum 3 characters")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email ID is required")]
        [RegularExpression("^[a-z]{3,}[.]*[a-z0-9]*[@]{1}[a-z]{2,}[.]{1}[co]{2}[m]*[.]*[a-z]*$", ErrorMessage = "Enter a valid email." )]
        public string EmailID { get; set; }
    }
}
