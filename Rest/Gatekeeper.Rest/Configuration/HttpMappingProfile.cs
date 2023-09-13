using AutoMapper;
using Gatekeeper.Core.Entities;
using Gatekeeper.Rest.Controllers;
using Gatekeeper.Rest.Dtos;

namespace Gatekeeper.Rest.Configuration;

public class HttpMappingProfile : Profile
{
    public HttpMappingProfile()
    {
        CreateMap<Unit, UnitResponse>();
    }
}