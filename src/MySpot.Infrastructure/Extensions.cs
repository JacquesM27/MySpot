﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Core.Abstractions;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Exceptions;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("app");
            services.Configure<AppOptions>(section);

            services.AddSingleton<ExceptionMIddleware>();

            services
                .AddPostgres(configuration)
                .AddSingleton<IClock, Clock>();

            var infraAssembly = typeof(AppOptions).Assembly;
            services.Scan(x => x.FromAssemblies(infraAssembly)
                .AddClasses(y => y.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMIddleware>();
            app.MapControllers();
            return app;
        }
    }
}
