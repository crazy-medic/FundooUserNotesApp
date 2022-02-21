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
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Enter your name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email ID is required")]
        public string EmailID { get; set; }
    }
}
