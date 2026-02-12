using Microsoft.Extensions.Options;
using FluentValidation;
using Order.Api.Data.Repositories;
using Order.Api.Data.Repositories.Interfaces;
using Order.Api.Services;
using Shared.Data.Extensions;
using Order.Api.Services.Interfaces;
using Order.Api.Validators;
using Messaging;
using Order.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<PubSubConfig>(builder.Configuration.GetSection(PubSubConfig.SectionName));

builder.Services.AddMessaging();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

// MongoDB Configuration
builder.Services.AddMongoDb(builder.Configuration);

builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

builder.Services.AddValidatorsFromAssemblyContaining<PedidoDtoValidator>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
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

