using System;
using System.Collections.Generic;

namespace sampledemo.Context;

public partial class Productpartner
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public int? Partnerid { get; set; }

    public int? Count { get; set; }

    public DateOnly? Date { get; set; }

    public virtual Partner? Partner { get; set; }

    public virtual Product? Product { get; set; }
}
