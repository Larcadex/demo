using System;
using System.Collections.Generic;

namespace sampledemo.Context;

public partial class Partner
{
    public int Id { get; set; }

    public int? Typepartner { get; set; }

    public string? Name { get; set; }

    public string? Director { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Inn { get; set; }

    public int? Rate { get; set; }

    public virtual ICollection<Productpartner> Productpartners { get; set; } = new List<Productpartner>();

    public virtual Partnertype? TypepartnerNavigation { get; set; }
}
