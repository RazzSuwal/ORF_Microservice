using Microservice.SharedLibrary.DependencyInjection;
using Microservice.SharedLibrary.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PropertyApi.Infrastructure.Data;
using PropertyApi.Infrastructure.Repositories;
using PropertyApi.Infrastructure.Repositories.Interfaces;

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
