using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.CustomerModels
{
   public class SignInInputModel
    {
       [Required(ErrorMessage = "Please enter your Email")]
       [StringLength(50, ErrorMessage = "The Email must be at least 5 to 50 characters long.", MinimumLength = 5)]
       [Display(Name = "Email Address")]
       [DataType(DataType.EmailAddress)]
       public string EmailAddress { get; set; }

       [Required(ErrorMessage = "Please enter Password")]
       [StringLength(11, ErrorMessage = "Maximum 11 characters exceeded", MinimumLength = 7)]
       [RegularExpression(@"^[^\s^<>]*$", ErrorMessage = "Input valid characters")]
       [Display(Name = "Password")]
       [DataType(DataType.Password)]
       public string Password { get; set; }
    }
}
