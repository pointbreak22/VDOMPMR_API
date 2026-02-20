using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityDb");

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.GetName().Name));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddOpenIddict()
     .AddCore(options =>
     {
         options.UseEntityFrameworkCore()
                .UseDbContext<AppIdentityDbContext>();
     })
     .AddServer(options =>
     {
         options.SetTokenEndpointUris("/connect/token");
         options.AllowPasswordFlow(); // нужен для Angular SPA
         options.AllowRefreshTokenFlow(); // если хочешь refresh tokens
         options.AcceptAnonymousClients(); // ← ОБЯЗАТЕЛЬНО для SPA
         options.RegisterScopes("openid", "email", "roles", "resource_api");

         options.AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate();

         options.UseAspNetCore()
                .EnableTokenEndpointPassthrough().EnableAuthorizationEndpointPassthrough();
     })
     .AddValidation(options =>
     {
         options.UseLocalServer();
         options.UseAspNetCore();
     });

            return services;
        }
    }
}
