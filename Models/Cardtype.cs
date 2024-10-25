using System;
using System.Collections.Generic;

namespace HosAuthenAPI.Models;

public partial class Cardtype
{
    public string Cardtype1 { get; set; } = null!;

    public string? Editmask { get; set; }

    public string? Name { get; set; }

    public string? HosGuid { get; set; }

    public string? HosGuidExt { get; set; }

    public string? NeedExpireDate { get; set; }
}
