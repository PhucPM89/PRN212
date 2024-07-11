using System;
using System.Collections.Generic;

namespace ProjectPRN2.Models;

public partial class LopHoc
{
    public string IdlopHoc { get; set; } = null!;

    public string? TenLopHoc { get; set; }

    public int? SlhocSinh { get; set; }

    public string? Gvcn { get; set; }
}
