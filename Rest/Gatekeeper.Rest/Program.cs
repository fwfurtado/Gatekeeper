using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
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
using Gatekeeper.Rest.Features.Package.List;
using Gatekeeper.Rest.Features.Package.Receive;
using Gatekeeper.Rest.Features.Package.Remove;
using Gatekeeper.Rest.Features.Package.Show;
using Gatekeeper.Shared.Database;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.Configure<JsonOptions>(option =>
{
    option.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    option.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    option.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
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


builder.Services.AddScoped<IPackageSaver, Gatekeeper.Rest.DataLayer.PackageRepository>();
builder.Services.AddScoped<IPackageFetcherByDescription, Gatekeeper.Rest.DataLayer.PackageRepository>();
builder.Services.AddScoped<IPackageListFetcher, Gatekeeper.Rest.DataLayer.PackageRepository>();
builder.Services.AddScoped<IPackageFetcherById, Gatekeeper.Rest.DataLayer.PackageRepository>();
builder.Services.AddScoped<IPackageRemover, Gatekeeper.Rest.DataLayer.PackageRepository>();
builder.Services.AddScoped<IValidator<ReceivePackageCommand>, ReceivePackageCommandValidator>();


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