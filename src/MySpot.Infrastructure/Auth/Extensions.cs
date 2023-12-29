using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.Security;
using MySpot.Infrastructure.DAL;
using System.Security.Claims;
using System.Text;

namespace MySpot.Infrastructure.Auth
{
    internal static class Extensions
    {
        private const string SectionName = "auth";
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthOptions>(configuration.GetRequiredSection(SectionName));
            var options = configuration.GetSection<AuthOptions>(SectionName);

            services
                .AddSingleton<IAuthenticator, Authenticator>()
                .AddScoped<ITokenStorage, HttpContextTokenStorage>()
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.Audience = options.Audience;
                    x.IncludeErrorDetails = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = options.Issuer,
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey)),
                    };
                });

            services
                .AddAuthorizationBuilder()
                .AddPolicy("is-admin", policy =>
                {
                    policy.RequireRole("admin");
                    //policy.RequireRole("admin").RequireUserName("stefan").RequireClaim("some claim").Requirements.Add(new MinimumAgeRequirement(21));
                });

            return services;
        }
    }

    public class MinimumAgeRequirement(int minimumAge) : IAuthorizationRequirement
    {
        public int MinimumAge => minimumAge;
    }

    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var dateOfBirth = context.User.FindFirst(
                c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == "MySpot-Issuer");

            if (dateOfBirth is null)
                return Task.CompletedTask;

            var userAge = CalculateAge(dateOfBirth.Value);

            if (userAge >= requirement.MinimumAge)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }

        private static int CalculateAge(string? dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(dateOfBirth))
                return 0;

            var birthDate = DateTime.Parse(dateOfBirth);
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
