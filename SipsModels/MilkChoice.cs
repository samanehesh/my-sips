using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class MilkChoice
{
    public int MilkChoiceId { get; set; }

    public string MilkType { get; set; } = null!;

    public decimal PriceModifier { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
