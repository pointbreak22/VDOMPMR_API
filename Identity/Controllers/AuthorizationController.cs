using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Identity.Controllers;

[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorizationController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest();

        if (request.IsPasswordGrantType())
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Forbid();

            var identity = new ClaimsIdentity(
                TokenValidationParameters.DefaultAuthenticationType,
                Claims.Name,
                Claims.Role);

            // 🔥 ОБЯЗАТЕЛЬНО
            identity.AddClaim(Claims.Subject, user.Id);

            // имя
            identity.AddClaim(Claims.Name, user.UserName);

            // роли
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                identity.AddClaim(Claims.Role, role);
            }

            var principal = new ClaimsPrincipal(identity);

            principal.SetScopes(request.GetScopes());
            principal.SetResources("resource_api");

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new InvalidOperationException("Grant type not supported.");
    }

    private static IEnumerable<string> GetDestinations(System.Security.Claims.Claim claim, string[] scopes)
    {
        switch (claim.Type)
        {
            case Claims.Name:
            case Claims.Subject:
                return new[] { Destinations.AccessToken };

            case Claims.Email:
                if (scopes.Contains(Scopes.Email))
                    return new[] { Destinations.AccessToken };
                break;

            case Claims.Role:
                if (scopes.Contains(Scopes.Roles))
                    return new[] { Destinations.AccessToken };
                break;
        }

        return new[] { Destinations.AccessToken };
    }
}
