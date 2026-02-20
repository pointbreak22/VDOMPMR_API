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

        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Данные не заполнены");

            // Используем Email как UserName, если Login пустой
            var userName = string.IsNullOrEmpty(model.Login) ? model.Email : model.Login;

            var user = new ApplicationUser { UserName = userName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                string roleName = (model.Email == "admin@admin.com") ? "Admin" : "user";

                // Используй _roleManager, который ты уже внедрил в конструктор!
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }

                await _userManager.AddToRoleAsync(user, roleName);
                return Ok();
            }

            // Если ошибка в пароле или почте, вернем 400 вместо 500
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

        //// GET: api/account/external-login/google
        //[HttpGet("external-login/google")]
        //public IActionResult ExternalLoginGoogle(string returnUrl = "/")
        //{
        //    var redirectUrl = Url.Action(nameof(ExternalLoginGoogleCallback), "Account", new { returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        //    return Challenge(properties, "Google");
        //}

        //// GET: api/account/external-login/google-callback
        //[HttpGet("external-login/google-callback")]
        //public async Task<IActionResult> ExternalLoginGoogleCallback(string? returnUrl = null)
        //{
        //    var info = await _signInManager.GetExternalLoginInfoAsync();
        //    if (info == null) return BadRequest("Ошибка Google Auth");

        //    var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

        //    ApplicationUser user;
        //    if (signInResult.Succeeded)
        //    {
        //        user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        //    }
        //    else
        //    {
        //        // Создаем пользователя, если его нет
        //        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        //        user = new ApplicationUser { UserName = email, Email = email };
        //        await _userManager.CreateAsync(user);
        //        await _userManager.AddLoginAsync(user, info);
        //        // По умолчанию даем роль user
        //        await _userManager.AddToRoleAsync(user, "user");
        //    }

        //    // ВАЖНО: После входа через Google в SPA, обычно делают редирект на Angular 
        //    // с токеном в URL или используют Authorization Code Flow.
        //    // Для начала просто верните результат:
        //    return Redirect($"https://localhost:4200/login-success?email={user.Email}");
        //}
    }


}
