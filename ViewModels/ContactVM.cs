using Sips.SipsModels;
using System.ComponentModel.DataAnnotations;

namespace Sips.ViewModels
{
    public class ContactVM
    {
        [Display(Name = "User ID")]

        public int? UserId { get; set; }
        [Display(Name = "First Name")]

        public string? FirstName { get; set; } = null!;
        [Display(Name = " Last Name")]

        public string? LastName { get; set; }
        [Display(Name = "Phone Number")]

        public string? PhoneNumber { get; set; } = null!;

        public string? Email { get; set; } = null!;

        public int? Unit { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }
        [Display(Name = "Postal Code")]

        public string? PostalCode { get; set; }
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM, yyyy}", ApplyFormatInEditMode = false)]

        public DateTime? BirthDate { get; set; }

        public bool? IsDrinkRedeemed { get; set; }

    }
}
