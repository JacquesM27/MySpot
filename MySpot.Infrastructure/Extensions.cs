using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Time;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.Repositories;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>()
                .AddSingleton<IClock, Clock>();
            
            return services;
        }
    }
}
