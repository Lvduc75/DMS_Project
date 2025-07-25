using System;
using System.Collections.Generic;

namespace DMS.Models.Entities;

public partial class ConfigPrice
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public DateOnly EffectiveFrom { get; set; }
}
