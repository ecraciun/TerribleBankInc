using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using TerribleBankInc.Models.ViewModels;
using IAuthenticationService=TerribleBankInc.Services.Interfaces.IAuthenticationService;
using TerribleBankInc.Helpers;
using TerribleBankInc.Models.Dtos;
using TerribleBankInc.Models.OperationResults;

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
                    await HandleLogin(loginResult.ClientUser);
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
                    await HandleLogin(result.ClientUser);
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

        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Forgot(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                //TODO: implement it
            }

            return View(forgotPasswordViewModel);
        }

        private async Task HandleLogin(ClientUser user)
        {
            var identity = new ClaimsIdentity("Cookie");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.ClientId.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.IsAdmin ? Constants.AdminRole : Constants.MemberRole));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
    }
}