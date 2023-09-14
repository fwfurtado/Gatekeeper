using AutoMapper;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Entities;
using Gatekeeper.Rest.Controllers;
using Gatekeeper.Rest.Dtos;
using Keycloak.AuthServices.Sdk.Admin.Models;
using System.Reflection;

namespace Gatekeeper.Rest.Configuration;

public class HttpMappingProfile : Profile
{
    public HttpMappingProfile()
    {
        CreateMap<Unit, UnitResponse>();
        CreateMap<Resident, ResidentResponse>()
        .ConvertUsing(resident => new ResidentResponse { Document = resident.Document.Number, Name = resident.Name });
        CreateMap<RegisterResidentRequest, RegisterResidentCommand>();
    }
}