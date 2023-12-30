using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Abstractions;
using MySpot.Core.Abstractions;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.DAL;
using MySpot.Infrastructure.Exceptions;
using MySpot.Infrastructure.Logging;
using MySpot.Infrastructure.Security;
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
            services.AddSecurity();
            services.AddAuth(configuration);
            services.AddHttpContextAccessor();

            services
                .AddPostgres(configuration)
                .AddSingleton<IClock, Clock>();

            var infraAssembly = typeof(AppOptions).Assembly;
            services.Scan(x => x.FromAssemblies(infraAssembly)
                .AddClasses(y => y.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            services.AddCustomLogging();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations();
                swagger.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "MySpot API",
                    Version = "v1"
                });
            });

            return services;
        }

        public static WebApplication UseInfrastructure(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMIddleware>();
            app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseReDoc(doc =>
            {
                doc.RoutePrefix = "docs";
                doc.SpecUrl("/swagger/v1/swagger.json");
                doc.DocumentTitle = "MySpot API";
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
