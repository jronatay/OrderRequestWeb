//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityLibrary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Customer
    {
        public Customer()
        {
            this.Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter First Name")]
        [StringLength(20, ErrorMessage = "The First Name must be at least 2 to 20 characters long.", MinimumLength = 2)]
        [RegularExpression(@"^[^\d^`~!@#\$%\^&\*\(\)\-_\+=<>\?,;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)\-_\+=<>\?,;:\|/\[\]\{\}""]$", ErrorMessage = "Input valid characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter Last Name")]
        [StringLength(20, ErrorMessage = "The Last Name must be at least 2 to 20 characters long.", MinimumLength = 2)]
        [RegularExpression(@"^[^\d^`~!@#\$%\^&\*\(\)\-_\+=<>\?,;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)\-_\+=<>\?,;:\|/\[\]\{\}""]$", ErrorMessage = "Input valid characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        [StringLength(200, ErrorMessage = "The Address must be at least 15 to 200 characters long.", MinimumLength = 15)]
        [RegularExpression(@"^[^`~!@#\$%\^&\*\(\)_\+=<>\?,;:'\|/\[\]\{\}""]+[^`~!@#\$%\^&\*\(\)_\+=<>\?,;:\|/\[\]\{\}""]$", ErrorMessage = "Input valid characters")]
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [StringLength(200, ErrorMessage = "The Address must be at least 15 to 200 characters long.", MinimumLength = 15)]
        [RegularExpression(@"^[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?,;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?,;:\|/\[\]\{\}""]$", ErrorMessage = "Input valid characters")]
        [Display(Name = "Address 2 - Optional")]
        public string Address2 { get; set; }

        [Required(ErrorMessage = "Please enter ZipCode")]
        [StringLength(20, ErrorMessage = "The ZipCode must be at least 2 to 20 characters long.", MinimumLength = 2)]
        [RegularExpression(@"^[A-Z0-9]{2,5}$|^[A-Z0-9]{2,5}-[A-Z0-9]{2,4}$", ErrorMessage = "Input valid characters")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please enter City")]
        [StringLength(50, ErrorMessage = "The City must be at least 5 to 50 characters long.", MinimumLength = 5)]
        [RegularExpression(@"^[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:\|/\[\]\{\}""]$", ErrorMessage = "Input valid characters")]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(50, ErrorMessage = "The Region must be at least 5 to 50 characters long.", MinimumLength = 5)]
        [RegularExpression(@"^[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:\|/\[\]\{\}""]$", ErrorMessage = "Input valid characters")]
        [Display(Name = "Region - Optional")]
        public string Region { get; set; }

        //[StringLength(50, ErrorMessage = "The Country must be at least 5 to 50 characters long.", MinimumLength = 5)]
        //[RegularExpression(@"^[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:'\|/\[\]\{\}""]+[^\d^`~!@#\$%\^&\*\(\)_\+=<>\?;:\|/\[\]\{\}""]$", ErrorMessage = "Input valid characters")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [StringLength(50, ErrorMessage = "The Email must be at least 5 to 50 characters long.", MinimumLength = 5)]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a valid Email")]

        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        public System.DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(11, ErrorMessage = "Maximum 11 characters exceeded", MinimumLength = 7)]
        [RegularExpression(@"^[^\s^<>]*$", ErrorMessage = "Input valid characters")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
