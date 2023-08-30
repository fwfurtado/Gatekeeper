using AutoMapper;

namespace Gatekeeper.Core.Configurations;

public static class AutoMapperConfiguration
{
    public static MapperConfiguration Configure()
    {
        return new MapperConfiguration(cfg =>
        {
            cfg.ShouldUseConstructor = constructor => constructor.IsPublic;
            cfg.AddProfile<GlobalMappingProfile>();
        });
    }
}