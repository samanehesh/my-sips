using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public int Inventory { get; set; }

    public int? ItemTypeId { get; set; }

    public int? ImageId { get; set; }

    public bool HasMilk { get; set; }

    public virtual ImageStore? Image { get; set; }

    public virtual ItemType? ItemType { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
