using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using TerribleBankInc.Models.ViewModels;
using IAuthenticationService=TerribleBankInc.Services.Interfaces.IAuthenticationService;
using TerribleBankInc.Helpers;

namespace TerribleBankInc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var loginResult = await _authenticationService.LoginAsync(loginViewModel.Username, loginViewModel.Password);
                if (loginResult.IsSuccess)
                {
                    //TODO: handle auth

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, loginResult.ClientUser.ClientId.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, loginResult.ClientUser.FirstName));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, loginResult.ClientUser.LastName));
                    if (loginResult.ClientUser.IsAdmin)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, Constants.AdminRole));
                    }

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError("", loginResult.Message);
                }
            }

            return View(loginViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.RegisterAsync(registerViewModel);
                if (result.IsSuccess)
                {
                    //TODO: handle auth
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
                
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}