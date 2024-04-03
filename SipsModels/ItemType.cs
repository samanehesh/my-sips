using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class ItemType
{
    public int ItemTypeId { get; set; }

    public string ItemTypeName { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
