using System.ComponentModel.DataAnnotations;

namespace Gatekeeper.Frontend.Admin.Dtos;

public class PackageRequest
{
    public string Description { get; set; } = null;
    public long UnitId { get; set; }
}

public class PackageForm
{
    public string Description { get; set; } = null;
    public long UnitId { get; set; }
}

