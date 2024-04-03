using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class AddInOrderDetail
{
    public int AddInId { get; set; }

    public int OrderDetailId { get; set; }

    public int Quantity { get; set; }

    public virtual AddIn AddIn { get; set; } = null!;

    public virtual OrderDetail OrderDetail { get; set; } = null!;
}
