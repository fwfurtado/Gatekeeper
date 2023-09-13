using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.Specifications;
using Gatekeeper.Core.Validations;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Shared.Database;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();

builder.Services.AddControllers();
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


builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<ICpfSpecification, CpfSpecification>();
builder.Services.AddScoped<IValidator<RegisterUnitCommand>, RegisterUnitCommandValidator>();
builder.Services.AddScoped<IValidator<RegisterResidentCommand>, RegisterResidentCommandValidator>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IResidentService, ResidentService>();
builder.Services.AddScoped<IResidentRepository, ResidentRepository>();

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

app.Run();

#pragma warning disable S1118
public partial class Program { }
#pragma warning restore S1118