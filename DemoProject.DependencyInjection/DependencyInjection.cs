using DemoProject.DependencyInjection.Settings;
using DemoProject.Domain.Interfaces;
using DemoProject.Domain.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using DemoProject.Database.Connection;
using DemoProject.Database.Repositories;

namespace DemoProject.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailService, EmailService>();

            // Fix: Use Bind instead of Configure to map the configuration section to the DatabaseSettings class
            services.Configure<DatabaseSettings>(options => configuration.GetSection(nameof(DatabaseSettings)).Bind(options));

            services.AddScoped<IEmailRepository, EmailRepository>();

            services.AddScoped<IConnectionFactory, DatabaseFactory>();

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(options => configuration.GetSection(nameof(DatabaseSettings)).Bind(options));

            services.AddScoped<IConnectionFactory, DatabaseFactory>();

            return services;
        }

        public static IServiceCollection AddRabbitDependencies(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddMassTransit(x =>
            {
                if (env.IsProduction())
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var settings = context.GetRequiredService<IOptions<RabbitSettings>>().Value;

                        cfg.Host(settings.HostUrl, h =>
                        {
                            h.Username(settings.Username);
                            h.Password(settings.Password);
                        });
                    });
                }
                else
                {
                    x.UsingRabbitMq();
                }
            });

            return services;
        }
    }
}
