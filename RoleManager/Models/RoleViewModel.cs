using System.ComponentModel.DataAnnotations;

namespace RoleManager.Models
{
    public class RoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public string Description { get; set; }
    }
}
