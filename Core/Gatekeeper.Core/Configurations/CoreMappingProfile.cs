using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Configurations;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        CreateMap<RegisterResidentCommand, Resident>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter();
        CreateMap<RegisterUnitCommand, Unit>()
            .ForMember(u => u.Id, opt => opt.Ignore())
            .ForMember(u => u.Residents, opt => opt.Ignore());
    }
}