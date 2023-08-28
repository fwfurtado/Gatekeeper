using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Configurations;

public class GlobalMappingProfile : Profile
{
    public GlobalMappingProfile()
    {
        CreateMap<RegisterTenantCommand, Tenant>();
        CreateMap<RegisterUnitCommand, Unit>()
            .ConvertUsing(command => new Unit(command.Identifier));
    }
}