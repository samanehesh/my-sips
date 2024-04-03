using System.ComponentModel.DataAnnotations;

namespace Sips.ViewModels
{
    public class UserVM
    {
        [Key]
        [Required]
        public string Email { get; set; }
    }
}
