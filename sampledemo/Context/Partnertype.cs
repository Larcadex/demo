using System;
using System.Collections.Generic;

namespace sampledemo.Context;

public partial class Partnertype
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Partner> Partners { get; set; } = new List<Partner>();
}
