using MySpot.Application;
using MySpot.Infrastructure;
using MySpot.Core;
using MySpot.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Time;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure()
    .AddControllers();

var app = builder.Build();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MySpotDbContext>();
    dbContext.Database.Migrate();

    var weeklyParkingSpots = dbContext.WeeklyParkingSpots.ToList();
    if (weeklyParkingSpots.Count == 0)
    {
        var _clock = new Clock();
        weeklyParkingSpots = [
            new(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(_clock.Current()), "P1"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(_clock.Current()), "P2"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(_clock.Current()), "P3"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(_clock.Current()), "P4"),
            new(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(_clock.Current()), "P5"),
        ];
        dbContext.WeeklyParkingSpots.AddRange(weeklyParkingSpots);
        dbContext.SaveChanges();
    }
}

app.Run();
