using System.Reflection;
using Sips.SipsModels;

namespace Sips.ViewModels
{
    public class MenuItemVM
    {
        public string Title { get; set; } = "Milk Tea";
        public List<Item> Items { get; set; }
        public List<Ice> IceOptions { get; set; } // List of ice options
        public List<Sweetness> SweetnessOptions { get; set; } // List of sweetness options
        public List<MilkChoice> MilkOptions { get; set; } // List of milk options
        public List<AddIn> AddInOptions { get; set; } // List of addon options

        //public byte[]? ImageData { get; set; }
        //public string? ImageBase64 { get; set; }

        // Constructor to initialize lists
        public MenuItemVM()
        {
            IceOptions = new List<Ice>();
            SweetnessOptions = new List<Sweetness>();
            MilkOptions = new List<MilkChoice>();
            AddInOptions = new List<AddIn>();
        }
    }
}
