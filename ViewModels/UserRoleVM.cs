using System.ComponentModel.DataAnnotations;

namespace Sips.ViewModels
{
    public class UserRoleVM
    {
        [Key]
        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
