using System;
using System.Collections.Generic;

namespace sampledemo.Context;

public partial class Product
{
    public int Id { get; set; }

    public int? Typeproduct { get; set; }

    public string? Name { get; set; }

    public string? Article { get; set; }

    public float? Mincost { get; set; }

    public virtual ICollection<Productpartner> Productpartners { get; set; } = new List<Productpartner>();

    public virtual Productstype? TypeproductNavigation { get; set; }
}
