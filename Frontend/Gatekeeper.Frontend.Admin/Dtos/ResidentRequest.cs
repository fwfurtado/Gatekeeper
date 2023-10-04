using System.ComponentModel.DataAnnotations;

namespace Gatekeeper.Frontend.Admin.Dtos;

public class ResidentRequest
{
    public string Name { get; set; } = null!;
    public string Document { get; set; } = null!;
}

public class ResidentForm
{
    [Required]    
    public string Name { get; set; } = null!;
    
    [RegularExpression (@"[0-9]+")] 
    public string Document { get; set; } = null!;

}
