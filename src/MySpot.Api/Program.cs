using MySpot.Application;
using MySpot.Infrastructure;
using MySpot.Core;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure()
    .AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();
