using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace roleManager.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
      
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
