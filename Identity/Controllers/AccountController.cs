using Identity.Requests;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

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
                return BadRequest(new { message = "Данные не заполнены" });

            // 🔹 Login = Email, если Login пустой
            var userName = string.IsNullOrEmpty(model.Login) ? model.Email : model.Login;

            // Проверяем, существует ли уже пользователь
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return BadRequest(new { message = "Пользователь с таким email уже существует" });

            var user = new ApplicationUser { UserName = userName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { message = "Ошибка регистрации", errors });
            }

            string roleName = (model.Email == "admin@admin.com") ? "Admin" : "User";

            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole(roleName));

            await _userManager.AddToRoleAsync(user, roleName);

            return Ok(new { message = "Регистрация прошла успешно", email = user.Email, role = roleName });
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

            // 🔹 Создаём cookie для OpenIddict
            await _signInManager.SignInAsync(user, isPersistent: true);

            return Ok(new { message = "Login successful" });
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
