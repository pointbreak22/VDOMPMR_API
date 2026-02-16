using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Identity.Controllers;

public class AuthorizationController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorizationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("~/connect/token")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("OSC request error");

        if (request.IsPasswordGrantType())
        {
            var user = await _userManager.FindByNameAsync(request.Username!);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password!))
                return Unauthorized("Invalid user or password");

            // Создаем ClaimsPrincipal
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            // ВАЖНО: Прописываем дестинации, чтобы роли попали в JWT токен
            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
            }

            principal.SetScopes(new[] {      
                OpenIddictConstants.Scopes.OpenId,  
                OpenIddictConstants.Scopes.Email,            
                OpenIddictConstants.Scopes.Roles, 
                "resource_api"
             }.Intersect(request.GetScopes()));

            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        // Тут можно добавить Refresh Token и прочее
        return BadRequest("Flow not supported");
    }
}