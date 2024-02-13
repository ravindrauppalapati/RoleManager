using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace roleManager.Models
{
    public class RegisterViewModel
    {
         
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailAvailable",controller:"Accounts")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password",ErrorMessage ="do not Match")]
        public string? ConfirmPassword { get; set; }
    }
}
