using System;
using System.Collections.Generic;

namespace Sips.SipsModels;

public partial class ImageStore
{
    public int ImageId { get; set; }

    public string FileName { get; set; } = null!;

    public byte[] Image { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
