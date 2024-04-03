using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class Ice
{
    public int IceId { get; set; }

    public string IcePercent { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
