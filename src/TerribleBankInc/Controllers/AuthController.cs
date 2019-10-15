using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TerribleBankInc.Services;
using TerribleBankInc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using IAuthenticationService=TerribleBankInc.Services.IAuthenticationService;

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
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    ModelState.AddModelError("", loginResult.Message);
                }
            }

            return View(loginViewModel);

            //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //identity.AddClaim(new Claim(ClaimTypes.Name, user.Ssn));
            //identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
            //identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));

            //foreach (var role in user.Roles)
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, role.Role));
            //}

            //var principal = new ClaimsPrincipal(identity);
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
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