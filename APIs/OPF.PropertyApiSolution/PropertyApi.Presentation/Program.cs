using PropertyApi.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastrutureService(builder.Configuration);

var app = builder.Build();

app.UseInfrastructurePolicy();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    var authHeader = context.Request.Headers["Authorization"].ToString();
//    Console.WriteLine($"[DEBUG][PropertyAPI] Auth Header: {authHeader}");
//    await next();
//});

app.MapControllers();
app.Run();
