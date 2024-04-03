namespace Sips.SipsModels
{
    [Serializable]
    public class Cart
    {
        public List<Item> Items { get; set; }

        public Cart()
        {
            Items = new List<Item>();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item); 
        }

        public decimal GetTotal()
        {
            return Items.Sum(item => item.BasePrice);
        }
    }

}
