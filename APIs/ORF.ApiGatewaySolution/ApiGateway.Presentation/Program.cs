using ApiGateway.Presentation.Middleware;
using Microservice.SharedLibrary.DependencyInjection;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
JWTAuthenticationScheme.AddJWTAuthenticationScheme(builder.Services, builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});
var app = builder.Build();
app.UseCors();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseMiddleware<AttachSignatureToRequest>();
//app.Use(async (context, next) =>
//{
//    var authHeader = context.Request.Headers["Authorization"].ToString();
//    Console.WriteLine($"[DEBUG] Auth Header: {authHeader}");
//    await next();
//});
app.UseOcelot().Wait();
app.UseHttpsRedirection();
app.Run();

