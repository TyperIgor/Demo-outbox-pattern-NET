using DemoWorker;
using DemoProject.DependencyInjection;
using DemoWorker.Services;
using DemoProject.Domain.Interfaces; // Ensure this namespace is included for RabbitMQ support

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddRabbitDependencies(builder.Environment);
builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<PublishEvent>();

var host = builder.Build();
host.Run();
