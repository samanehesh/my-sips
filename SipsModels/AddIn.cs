using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class AddIn
{
    public int AddInId { get; set; }

    public string AddInName { get; set; } = null!;

    public decimal PriceModifier { get; set; }

    public virtual ICollection<AddInOrderDetail> AddInOrderDetails { get; set; } = new List<AddInOrderDetail>();
}
