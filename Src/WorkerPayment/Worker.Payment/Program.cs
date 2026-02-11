using WorkerPayment;

using Messaging;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMessaging();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
