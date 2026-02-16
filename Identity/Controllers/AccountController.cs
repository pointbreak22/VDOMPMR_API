using Identity.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверяем, есть ли роль user, если нет — создаём
            //const string defaultRole = "user";
            //var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
            //if (!await roleManager.RoleExistsAsync(defaultRole))
            //{
            //    await roleManager.CreateAsync(new IdentityRole(defaultRole));
            //}

            var user = new ApplicationUser { UserName = model.Login, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Логика: первый зарегистрированный или по спец. email становится админом
                string role = (model.Email == "admin@admin.com") ? "Admin" : "user";

                // Проверка существования роли
                var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(user, role);
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.Login);
            if (user == null)
                return Unauthorized("Invalid login or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return Unauthorized("Invalid login or password");

            // Здесь можно выдать токен через OpenIddict (password flow)
            // Обычно клиент делает отдельный запрос на /connect/token
            return Ok();
        }

        // GET: api/account/external-login/google
        [HttpGet("external-login/google")]
        public IActionResult ExternalLoginGoogle(string returnUrl = "/")
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginGoogleCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        // GET: api/account/external-login/google-callback
        [HttpGet("external-login/google-callback")]
        public async Task<IActionResult> ExternalLoginGoogleCallback(string? returnUrl = null)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null) return BadRequest("Ошибка Google Auth");

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            ApplicationUser user;
            if (signInResult.Succeeded)
            {
                user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            }
            else
            {
                // Создаем пользователя, если его нет
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                user = new ApplicationUser { UserName = email, Email = email };
                await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user, info);
                // По умолчанию даем роль user
                await _userManager.AddToRoleAsync(user, "user");
            }

            // ВАЖНО: После входа через Google в SPA, обычно делают редирект на Angular 
            // с токеном в URL или используют Authorization Code Flow.
            // Для начала просто верните результат:
            return Redirect($"https://localhost:4200/login-success?email={user.Email}");
        }
    }


}
