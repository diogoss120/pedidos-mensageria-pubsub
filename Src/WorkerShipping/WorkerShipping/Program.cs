using Shared.Data.Extensions;
using WorkerShipping;
using WorkerShipping.Configuration;
using WorkerShipping.Data.Repositories;
using WorkerShipping.Data.Repositories.Interfaces;
using WorkerShipping.Services;
using WorkerShipping.Services.Interfaces;

var builder = Host.CreateApplicationBuilder(args);

// MongoDB
builder.Services.AddMongoDb(builder.Configuration);

// PubSub
var pubSubConfig = builder.Configuration.GetSection("PubSubConfig").Get<PubSubConfig>();

if (pubSubConfig is null)
{
    throw new InvalidOperationException("PubSubConfig section is missing or invalid in appsettings.json");
}

builder.Services.AddSingleton<IEnvioRepository, EnvioRepository>();
builder.Services.AddSingleton<IEnvioService, EnvioService>();
builder.Services.AddSingleton<ICorreiosGateway, CorreiosGateway>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
