using Microservice.SharedLibrary.CURDHelper.Interfaces;
using Microservice.SharedLibrary.CURDHelper;
using Microservice.SharedLibrary.DependencyInjection;
using Microservice.SharedLibrary.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookingApi.Infrastructure.Data;
using Confluent.Kafka;
using BookingApi.Infrastructure.Respositories.Interfaces;
using BookingApi.Infrastructure.Respositories;

namespace BookingApi.Infrastructure.DependencyInjection
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

            services.AddScoped<IBookingRepo, BookingRepo>();
            services.AddScoped<ICURDHelper, CURDHelper>();

            var kafkaonfig = new ConsumerConfig 
            { 
                GroupId = "add-property-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            services.AddSingleton<IConsumer<Null, string>>
                (x => new ConsumerBuilder<Null, string>(kafkaonfig).Build());

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
