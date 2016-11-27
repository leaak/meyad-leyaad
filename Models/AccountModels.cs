using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace MeyadLeyaad1.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
       // [DataType(DataType.EmailAddress)]
     //   [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
      //  ErrorMessage = "כתובת דואר אלקטרוני אינה תקינה")]
        [Required(ErrorMessage="*")]
        [Display(Name = "שם משתמש")]
        public string User_Name { get; set; }

        [StringLength(100, ErrorMessage = " {2} הסיסמה צריכה להיות באורך ", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "סיסמה")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        //[Required]
        //[Display(Name = "First_Name")]
        //public string First_Name { get; set; }
        //[Required]
        //[Display(Name = "Last_Name")]
        //public string Last_Name { get; set; }
        //[Required]
        //[Display(Name = "Email")]
        //public string Email { get; set; }

        //[Required]
        //[Display(Name = "City")]
        //public string City { get; set; }

        //[Required]
        //[Display(Name = "Street")]
        //public string Street { get; set; }

        //[Required]
        //[Display(Name = "Building")]
        //public int Building { get; set; }

        //[Required]
        //[Display(Name = "House")]
        //public int House { get; set; }

        //[Required]
        //[Display(Name = "Floor")]
        //public int Floor { get; set; }

        //[Required]
        //[Display(Name = "Phone")]
        //public string Phone { get; set; }


        //[Display(Name = "Another_Phone")]
        //public string Another_Phone { get; set; }

        //[Display(Name = "Fax")]
        //public string Fax { get; set; }

        //[Display(Name = "Comments")]
        //public string Comments { get; set; }

        [Required(ErrorMessage="*")]
     //   [Display(Name= "שם משתמש")]
        public string User_Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
        ErrorMessage = "כתובת דואר אלקטרוני אינה תקינה")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = " {2} הסיסמה צריכה להיות באורך ", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "שדה זה אינו זהה לסיסמה שהוזנה")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
