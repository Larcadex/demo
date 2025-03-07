using System;
using System.Collections.Generic;

namespace sampledemo.Context;

public partial class Productstype
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public float? Kef { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
