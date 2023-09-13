using AutoMapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Rest.Controllers;

namespace Gatekeeper.Rest.Configuration;

public class HttpMappingProfile : Profile
{
    public HttpMappingProfile()
    {
        CreateMap<Unit, UnitResponse>();
    }
}