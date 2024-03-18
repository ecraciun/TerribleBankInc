using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TerribleBankInc.Helpers;
using TerribleBankInc.Models.Dtos;
using TerribleBankInc.Models.OperationResults;
using TerribleBankInc.Models.ViewModels;
using IAuthenticationService = TerribleBankInc.Services.Interfaces.IAuthenticationService;

namespace TerribleBankInc.Controllers;

public class AuthController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            LoginResult loginResult = await _authenticationService.LoginAsync(
                loginViewModel.Username,
                loginViewModel.Password
            );
            if (loginResult.IsSuccess)
            {
                await HandleLogin(loginResult.ClientUser);
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
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
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (ModelState.IsValid)
        {
            LoginResult result = await _authenticationService.RegisterAsync(registerViewModel);
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
            ForgotPasswordResult result = await _authenticationService.CreatePasswordForgetToken(
                forgotPasswordViewModel.Email
            );
            if (result.IsSuccess)
                return RedirectToAction(
                    nameof(ResetPassword),
                    new { token = result.ForgotPasswordToken }
                );
            else
                ModelState.AddModelError("", result.Message);
        }

        return View(forgotPasswordViewModel);
    }

    [HttpGet("[controller]/[action]/{token}")]
    public async Task<IActionResult> ResetPassword(string token)
    {
        if (
            string.IsNullOrEmpty(token)
            || !await _authenticationService.IsForgotPasswordTokenValid(token)
        )
            return BadRequest();

        return View(new ResetPasswordViewModel { Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
    {
        if (ModelState.IsValid)
        {
            OperationResult result = await _authenticationService.ResetPasswordWithToken(
                resetPasswordViewModel.Token,
                resetPasswordViewModel.Password
            );
            if (result.IsSuccess)
                return RedirectToAction(nameof(Login));
            else
                ModelState.AddModelError("", result.Message);
        }

        return View(resetPasswordViewModel);
    }

    private async Task HandleLogin(ClientUser user)
    {
        ClaimsIdentity identity = new("Cookie");
        identity.AddClaim(new Claim(ClaimTypes.Name, user.ClientId.ToString()));
        identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
        identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
        identity.AddClaim(
            new Claim(ClaimTypes.Role, user.IsAdmin ? Constants.AdminRole : Constants.MemberRole)
        );

        ClaimsPrincipal principal = new(identity);
        await HttpContext.SignInAsync(principal);
    }
}
