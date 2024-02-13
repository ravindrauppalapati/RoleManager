using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RoleManager.Models
{
    public class EditRoleViewModel
    {
        [Required]
        public string roleId { get; set; }
        
        [Required] 
        public string RoleName { get; set; }

        public string Description { get; set; }

        public List<string> Users { get; set; }
         
    }
}
