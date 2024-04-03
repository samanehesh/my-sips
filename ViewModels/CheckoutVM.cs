using Sips.SipsModels;
using System.ComponentModel.DataAnnotations;

namespace Sips.ViewModels
{
    public class CheckoutVM
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; } = null!;
        public decimal BasePrice { get; set; }
        public int Quantity { get; set; }
        public string AddInName { get; set; } = null!;
        public decimal AddInPriceModifier { get; set; }
        public string SweetnessPercent { get; set; }
        public string IcePercent { get; set; }
        public decimal Subtotal { get; set; }
        public string MilkType { get; set; }
        public decimal MilkPriceModifier { get; set; }
        public List<AddInOrderDetail> AddInNames { get; set; } = new List<AddInOrderDetail>();
        public List<AddInOrderDetail> AddInPriceModifiers { get; set; } = new List<AddInOrderDetail>();
    }
}