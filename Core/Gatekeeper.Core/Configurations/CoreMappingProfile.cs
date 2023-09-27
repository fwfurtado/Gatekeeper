using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;

namespace Gatekeeper.Core.Configurations;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        CreateMap<RegisterResidentCommand, Resident>()
            .IgnoreAllPropertiesWithAnInaccessibleSetter()
            .ForMember(u => u.Id, opt => opt.Ignore());
        CreateMap<RegisterUnitCommand, Unit>()
            .ForMember(u => u.Id, opt => opt.Ignore());
        CreateMap<Unit, TargetUnit>()
            .ForMember(t => t.UnitId, opt => opt.MapFrom(u => u.Id));
        CreateMap<PersonalInfo, Resident>();
        CreateMap<NewOccupationCommand, OccupationRequest>();
    }
}