using FluentValidation;
using Gatekeeper.Core.Commands;
using Gatekeeper.Core.Configurations;
using Gatekeeper.Core.Infra;
using Gatekeeper.Core.Policies;
using Gatekeeper.Core.Repositories;
using Gatekeeper.Core.Services;
using Gatekeeper.Core.Validations;
using Gatekeeper.Rest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(cfg =>
{
    cfg.ShouldUseConstructor = constructor => constructor.IsPublic;
    cfg.AddProfile<GlobalMappingProfile>();
});

builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();