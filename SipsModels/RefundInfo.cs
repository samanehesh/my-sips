using System.ComponentModel.DataAnnotations;

namespace Sips.SipsModels
{
    public class RefundInfo
    {
        [Display(Name = "Refund Id:")]
        public string Id { get; set; }

        [Display(Name = "Status:")]
        public string Status { get; set; }

        public PaymentNotification PaymentNotification { get; set; }
    }

}
