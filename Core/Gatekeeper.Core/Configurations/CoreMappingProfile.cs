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
        CreateMap<RegisterPackageCommand, Package>()
            .ForMember(p => p.Id, opt => opt.Ignore())
            .ForMember(p => p.ArrivedAt, opt => opt.Ignore())
            .ForMember(p => p.DeliveredAt, opt => opt.Ignore())
            .ForMember(p => p.Status, opt => opt.Ignore())
            .ForCtorParam("description", opt => opt.MapFrom(c => c.Description));
    }
}