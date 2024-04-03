using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreHours { get; set; } = null!;

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
