using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Carter;
using Dapper;
using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Events.Handlers;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.Specifications;
using Gatekeeper.Core.Validations;
using Gatekeeper.Rest;
using Gatekeeper.Rest.Configuration;
using Gatekeeper.Rest.Consumers;
using Gatekeeper.Rest.Consumers.PushNotification;
using Gatekeeper.Rest.DataLayer;
using Gatekeeper.Rest.EventHandlers;
using Gatekeeper.Rest.Factories;
using Gatekeeper.Rest.Features.Notification.Create;
using Gatekeeper.Rest.Features.Notification.Send;
using Gatekeeper.Rest.Features.Package.List;
using Gatekeeper.Rest.Features.Package.Receive;
using Gatekeeper.Rest.Features.Package.Reject;
using Gatekeeper.Rest.Features.Package.Remove;
using Gatekeeper.Rest.Features.Package.Show;
using Gatekeeper.Rest.Infra;
using Gatekeeper.Rest.Infra.Aws;
using Gatekeeper.Rest.Infra.Dapper;
using Gatekeeper.Shared.Database;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk.Admin;
using MassTransit;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddControllers();
builder.Services.Configure<JsonOptions>(option =>
{
    option.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    option.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    option.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});


builder.Services.AddTransient<ISerializerDataContractResolver>(p =>
{
    var jsonOptions = p.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions;
    return new JsonSerializerDataContractResolver(jsonOptions);
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





builder.Services.AddMassTransit(m =>
{
    m.AddConsumer<NotificationConsumer>();

    m.UsingAmazonSqs((context, configurator) =>
    {
        var notificationSetting = builder.Configuration.GetSection(NotificationSettings.SectionName).Get<NotificationSettings>();

        if (notificationSetting is null)
        {
            throw new InvalidStateException("Notification settings not found in configuration");
        }

        if (builder.Environment.IsDevelopment())
        {
            configurator.LocalstackHost();
        }

        configurator.Message<NotificationSent>( t => {
            t.SetEntityName(notificationSetting.TopicName);
        });


        configurator.ReceiveEndpoint(notificationSetting.Push.QueueName, endpoint =>
        {
            endpoint.ConfigureConsumeTopology = false;

            endpoint.Subscribe(notificationSetting.TopicName);
        });


        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<IUnitRepository, UnitRepository>();

builder.Services.AddScoped<ICpfSpecification, CpfSpecification>();
builder.Services.AddScoped<IValidator<RegisterUnitCommand>, RegisterUnitCommandValidator>();
builder.Services.AddScoped<IValidator<RegisterResidentCommand>, RegisterResidentCommandValidator>();
builder.Services.AddScoped<IValidator<RegisterPackageCommand>, RegisterPackageCommandValidator>();
builder.Services.AddScoped<IUnitService, UnitService>();

builder.Services.AddScoped<IResidentService, ResidentService>();
builder.Services.AddScoped<IResidentRepository, ResidentRepository>();
builder.Services.AddScoped<IOccupationRequestRepository, OccupationRequestRepository>();
builder.Services.AddScoped<IOccupationRepository, OccupationRepository>();
builder.Services.AddScoped<IOccupationRequestEffectiveUnitOfWork, OccupationRequestEffectiveUnitOfWork>();
builder.Services.AddScoped<IOccupationService, OccupationService>();
builder.Services.AddScoped<NewOccupationCommandFactory>();


builder.Services.AddScoped<IPackageSaver, PackageRepository>();
builder.Services.AddScoped<IPackageFetcherByDescription, PackageRepository>();
builder.Services.AddScoped<IPackageListFetcher, PackageRepository>();
builder.Services.AddScoped<IPackageFetcherById, PackageRepository>();
builder.Services.AddScoped<IPackageSyncStatus, PackageRepository>();
builder.Services.AddScoped<IPackageRemover, PackageRepository>();
builder.Services.AddScoped<IValidator<ReceivePackageCommand>, ReceivePackageCommandValidator>();
builder.Services.AddScoped<IPackageEventSaver, PackageEventRepository>();
builder.Services.AddScoped<IPackageStateMachineFactory, PackageStateMachineFactory>();

builder.Services.AddScoped<INotificationSaver, NotificationRepository>();
builder.Services.AddScoped<INotificationFetcher, NotificationRepository>();
builder.Services.AddScoped<ISendNotificationRepository, SendNotificationRepository>();

builder.Services.AddScoped<IValidator<CreateNotificationCommand>, CreateNotificationValidator>();

builder.Services.AddSingleton<IJsonSerializer, DefaultJsonSerializer>();
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddCarter();
builder.Services.AddAws(builder.Environment);

var app = builder.Build();


SqlMapper.AddTypeHandler(new JsonTypeHandler<Dictionary<string, object>>(app.Services.GetRequiredService<IJsonSerializer>()));

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
