using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class Rating
{
    public int RatingId { get; set; }

    public string Rating1 { get; set; } = null!;

    public DateTime Date { get; set; }

    public string Comment { get; set; } = null!;

    public int StoreId { get; set; }

    public int UserId { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual Contact User { get; set; } = null!;
}
