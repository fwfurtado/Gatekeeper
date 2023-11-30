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
            .ForMember(t => t.Id, opt => opt.Ignore())
            .ForMember(t => t.Occupation, opt => opt.Ignore());
        CreateMap<Unit, TargetUnit>()
            .ForMember(t => t.UnitId, opt => opt.MapFrom(u => u.Id));
        CreateMap<PersonalInfo, Resident>()
            .ForMember(t => t.Id, opt => opt.Ignore());
        CreateMap<NewOccupationCommand, OccupationRequest>()
            .ForMember(t => t.Id, opt => opt.Ignore())
            .ForMember(t => t.Status, opt => opt.Ignore());
    }
}