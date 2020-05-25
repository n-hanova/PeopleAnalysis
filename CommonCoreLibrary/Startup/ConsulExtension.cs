using CommonCoreLibrary.Startup.Settings;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace CommonCoreLibrary.Startup
{
    public static class ConsulExtension
    {
        public static void AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            var consulSettings = configuration.GetSection("consulConfig").Get<ConsulSettings>();
            services.AddSingleton(consulSettings);

            services.AddSingleton<IConsulClient, ConsulClient>(p =>
                            new ConsulClient(consulConfig => consulConfig.Address = new Uri(consulSettings.Address)));
        }

        public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {

            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var consulConfig = app.ApplicationServices.GetRequiredService<ConsulSettings>();

            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();

            var features = app.Properties["server.Features"] as FeatureCollection;
            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            var port = address.Substring(address.LastIndexOf(":") + 1);
            if (port.Last() == '/')
                port = port[0..^1];

            var registration = new AgentServiceRegistration()
            {
                ID = $"{consulConfig.ServiceId}",
                Name = consulConfig.ServiceName,
                Address = configuration["service:name"] ?? address,
                Port = int.Parse(port),
                Tags = consulConfig.Tags?.ToArray() ?? new string[0]
            };

            logger.LogInformation("Registered with Consul");

            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
