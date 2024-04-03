using System.ComponentModel.DataAnnotations;

namespace Sips.ViewModels
{
    public class PayPalVM
    {
        [Key]
        [Display(Name = "Payment ID")]
        public string TransactionId { get; set; }
        public string Amount { get; set; }
        [Display(Name = "Name")]
        public string PayerName { get; set; }
        [Display(Name = "Email")]
        public string PayerEmail { get; set; }
        [Display(Name = "Created")]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "MOP")]
        public string PaymentMethod { get; set; }
        public string Currency { get; set; }

        public bool isFullfilled { get; set; } = false;

        // Nullable properties from PaymentNotification

        [Display(Name = "Currency Symbol")]
        public string? CurrencySymbol { get; set; }

        [Display(Name = "Payer Id")]
        public string? PayerId { get; set; }

        [Display(Name = "Payer Name")]
        public string? PayerFullName { get; set; }

        [Display(Name = "Capture Id")]
        public string? CaptureId { get; set; }
    }
}
