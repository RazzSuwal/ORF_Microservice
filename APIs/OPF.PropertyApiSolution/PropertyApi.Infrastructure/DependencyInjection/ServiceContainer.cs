using Microservice.SharedLibrary.CURDHelper.Interfaces;
using Microservice.SharedLibrary.CURDHelper;
using Microservice.SharedLibrary.DependencyInjection;
using Microservice.SharedLibrary.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyApi.Infrastructure.Data;
using PropertyApi.Infrastructure.Repositories;
using PropertyApi.Infrastructure.Repositories.Interfaces;
using Confluent.Kafka;

namespace PropertyApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastrutureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity
            services.AddScoped<IDatabaseFactory>(provider =>
            {
                var connectionString = config.GetConnectionString("OPFConnection");
                return new DatabaseFactory(connectionString!);
            });

            // Add authentication scheme
            SharedServiceContainer.AddSharedService(services, config, config["MySerialog:FileName"]!);

            // Create Dependency Injection

            services.AddScoped<IPropertyRepo, PropertyRepo>();
            services.AddScoped<ICURDHelper, CURDHelper>();

            var kafkaonfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
            services.AddSingleton<IProducer<Null, string>>
                (x=> new ProducerBuilder<Null,string>(kafkaonfig).Build());

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            // Register middleware such as:
            // Global Expection: handles external errors
            // Listen to Only Api Gateway

            SharedServiceContainer.UseSharedPolicies(app);
            return app;
        }
    }
}
