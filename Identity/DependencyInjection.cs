using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

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

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None; // критично для localhost + cross-site
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // обязательно для SameSite=None
            });

            services.AddOpenIddict()  
                .AddCore(options =>             
                {                               
                    options.UseEntityFrameworkCore()  
                    .UseDbContext<AppIdentityDbContext>(); 
                })                                  
                .AddServer(options =>            
                {                                                   
                    options.SetAuthorizationEndpointUris("/connect/authorize");  
                    options.SetTokenEndpointUris("/connect/token");   
                    
                    options.AllowAuthorizationCodeFlow();      // 🔥 вместо password
                    options.RequireProofKeyForCodeExchange();  // PKCE обязателен

                    options.AllowRefreshTokenFlow(); // если хочешь refresh tokens
                    options.AcceptAnonymousClients(); // ← ОБЯЗАТЕЛЬНО для SPA
                    options.RegisterScopes(     
                        Scopes.OpenId,          
                        Scopes.Profile,          
                        Scopes.Email,             
                        Scopes.Roles,        
                        "resource_api"); 
                    options.AddDevelopmentEncryptionCertificate()   
                    .AddDevelopmentSigningCertificate();   
                    options.UseAspNetCore()             
                    .EnableTokenEndpointPassthrough()      
                    .EnableAuthorizationEndpointPassthrough();  
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
