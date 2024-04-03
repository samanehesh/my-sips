using Sips.SipsModels;
using System.ComponentModel.DataAnnotations;

namespace Sips.ViewModels
{
    public class OrderDetailVM
    {
        [Display(Name = "Order Detail Ids")]

        public List<int>? OrderDetailIds { get; set; }
        [Display(Name = "Item Types")]

        public List<string>? ItemTypes { get; set; }
        [Display(Name = "Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]


        //public string? OrderDetailId { get; set; }

        public double? totalPrice { get; set; }

        //public int? Quantity { get; set; }
        [Display(Name = "Transaction Id")]

        public string? TransactionId { get; set; }
        [Display(Name = "Date Ordered")]
        [DisplayFormat(DataFormatString = "{0:dd MMM, yyyy}", ApplyFormatInEditMode = false)]


        public DateTime? DateOrdered { get; set; }
        [Display(Name = "Store Id")]

        public int? StoreId { get; set; }
        [Display(Name = "User Id")]

        public int? UserId { get; set; }
        [Display(Name = "Status Id")]

        public int? StatusId { get; set; }
        [Display(Name = "User Email")]

        public string? UserEmail { get; set; }
        [Display(Name = "Total Quantity")]

        public int? totalQuantity { get; set; }
        [Display(Name = "Order Details")]

        public List<OrderDetail>? orderDetail { get; set; }
        public List<int>? addInIds { get; set; }

        public Dictionary<int, List<int>>? addInIdsDictionary { get; set; }


        //public string? StoreHours { get; set; } 

    }
}
