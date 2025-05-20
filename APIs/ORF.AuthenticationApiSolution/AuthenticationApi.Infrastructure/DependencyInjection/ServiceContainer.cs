using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using AuthenticationApi.Infrastructure.Repositories.Interfaces;
using Microservice.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity
            services.AddDbContext<AuthenticationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("OPFConnection"));
            });

            // Add authentication scheme
            SharedServiceContainer.AddSharedService(services, config, config["MySerialog:FileName"]!);

            // Create Dependency Injection

            services.AddScoped<IUser, UserRepo>();

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
