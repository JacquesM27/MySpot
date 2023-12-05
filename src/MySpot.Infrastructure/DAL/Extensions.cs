using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.DAL.Repositories;

namespace MySpot.Infrastructure.DAL
{
    internal static class Extensions
    {
        private const string sectionName = "postgres";
        internal static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(sectionName);
            services.Configure<PostgresOptions>(section);//push into the container ioc
            var options = configuration.GetSection<PostgresOptions>(sectionName);//get locally

            
            services.AddDbContext<MySpotDbContext>(x => x.UseNpgsql(options.ConnectionString));
            services.AddScoped<IWeeklyParkingSpotRepository, PostgresWeeklyParkingSpotRepository>();
            services.AddHostedService<DatabaseInitializer>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return services;
        }

        public static T GetSection<T>(this IConfiguration configuration, string sectionName) 
            where T : class, new()
        {
            var options = new T();
            var section = configuration.GetSection(sectionName);
            section.Bind(options);
            return options;
        }
    }
}
