using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        // 1️⃣ Роли
        string[] roles = { "Admin", "user" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2️⃣ Админ
        var adminEmail = "admin@admin.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var admin = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        // 3️⃣ OpenIddict Client
       if (await appManager.FindByClientIdAsync("angular_spa") == null)
{
    await appManager.CreateAsync(new OpenIddictApplicationDescriptor
    {
        ClientId = "angular_spa",
        DisplayName = "Angular SPA",

        Permissions =
        {
            // endpoints
            Permissions.Endpoints.Token,

            // grant types
            Permissions.GrantTypes.Password,
            Permissions.GrantTypes.RefreshToken,

            // scopes ❗ ВАЖНО
            Permissions.Prefixes.Scope + Scopes.OpenId,
            Permissions.Prefixes.Scope + Scopes.Email,
            Permissions.Prefixes.Scope + Scopes.Roles,
            Permissions.Prefixes.Scope + "resource_api"
        }
    });
}
        }
    }

