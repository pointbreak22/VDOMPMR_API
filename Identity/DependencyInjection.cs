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
                options.Cookie.Name = "AUTH_SESSION"; // Понятное имя
                options.Cookie.HttpOnly = true;       // JS не видит куку (безопасно)
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.Path = "/";


              

                options.Events.OnRedirectToLogin = context =>
                {
                    // Если запрос идет к эндпоинтам OpenIddict, не нужно принудительно ставить 401,
                    // иначе механизм Challenge в контроллере не сработает.
                    // Если это API запрос (начинается с /api или /connect)
                    if (context.Request.Path.StartsWithSegments("/api") ||
                        context.Request.Path.StartsWithSegments("/connect"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    }
                    else
                    {
                        // Позволяем стандартный редирект для эндпоинтов авторизации
                        context.Response.Redirect(context.RedirectUri);
                    }
                    return Task.CompletedTask;
                };
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
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableStatusCodePagesIntegration(); // Помогает корректно пробрасывать 401/404
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
