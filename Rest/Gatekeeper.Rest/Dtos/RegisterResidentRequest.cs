using Gatekeeper.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatekeeper.Rest.Dtos;

public class RegisterResidentRequest
{
    public string Name { get; set; }
    public string Document { get; set; }
}