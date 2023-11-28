using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Events.Handlers;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.Specifications;
using Gatekeeper.Core.Validations;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.Factories;
using Gatekeeper.Shared.Database;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Carter;
using FluentValidation.AspNetCore;
using Gatekeeper.Rest.Features.Package.Receive;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();

builder.Services
    .AddControllers()
    .AddJsonOptions(option =>
{
    option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var authenticationOptions = builder
    .Configuration
    .GetSection(KeycloakAuthenticationOptions.Section)
    .Get<KeycloakAuthenticationOptions>()!;

builder.Services.AddKeycloakAuthentication(authenticationOptions);

var authorizationOptions = builder
    .Configuration
    .GetSection(KeycloakProtectionClientOptions.Section)
    .Get<KeycloakProtectionClientOptions>()!;

builder.Services.AddKeycloakAuthorization(authorizationOptions);

var adminClientOptions = builder
    .Configuration
    .GetSection(KeycloakAdminClientOptions.Section)
    .Get<KeycloakAdminClientOptions>()!;

builder.Services.AddKeycloakAdminHttpClient(adminClientOptions);


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OpenIdConnect,
        OpenIdConnectUrl = new Uri($"{authenticationOptions.KeycloakUrlRealm}/.well-known/openid-configuration")
    });


    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "oauth2",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});


builder.Services.AddAutoMapper(cfg =>
{
    cfg.ShouldUseConstructor = constructor => constructor.IsPublic;
    cfg.AddProfile<CoreMappingProfile>();
    cfg.AddProfile<HttpMappingProfile>();
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.RegisterServicesFromAssemblyContaining<OccupationRequestApprovedHandler>();
});

DapperConfiguration.Configure();

const string corsPolicyName = "CorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
        policy
            .WithOrigins("http://localhost:5000", "https://localhost:5032")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
    );
});

builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<ICpfSpecification, CpfSpecification>();
builder.Services.AddScoped<IValidator<RegisterUnitCommand>, RegisterUnitCommandValidator>();
builder.Services.AddScoped<IValidator<RegisterResidentCommand>, RegisterResidentCommandValidator>();
builder.Services.AddScoped<IValidator<RegisterPackageCommand>, RegisterPackageCommandValidator>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<IResidentService, ResidentService>();
builder.Services.AddScoped<IResidentRepository, ResidentRepository>();
builder.Services.AddScoped<IOccupationRequestRepository, OccupationRequestRepository>();
builder.Services.AddScoped<IOccupationRepository, OccupationRepository>();
builder.Services.AddScoped<IOccupationRequestEffectiveUnitOfWork, OccupationRequestEffectiveUnitOfWork>();
builder.Services.AddScoped<IOccupationService, OccupationService>();
builder.Services.AddScoped<NewOccupationCommandFactory>();
builder.Services.AddScoped<IReceiveRepository, ReceiveRepository>();
builder.Services.AddScoped<IValidator<ReceivePackageCommand>, ReceivePackageRequestValidator>();


builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(authenticationOptions.Resource);
        options.OAuthClientSecret(authenticationOptions.Credentials.Secret);
        options.OAuthScopes("openid");
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors(corsPolicyName);

app.MapCarter();

app.Run();

#pragma warning disable S1118
public partial class Program
{
}
#pragma warning restore S1118