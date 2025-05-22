using Microservice.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Microservice.SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedService
            (this IServiceCollection services, IConfiguration config, string fileName)
        {
            //configure
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();


            // Add JWT authentication scheme
            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);

            // Create Dependency Injection


            return services;

        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            // Userglobal Exception
            app.UseMiddleware<GlobalException>();
            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
