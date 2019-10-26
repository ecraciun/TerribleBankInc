using System;
using System.Diagnostics;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Boom()
        {
            HttpContext.RiseError(new NotImplementedException("Forgot something?"));
            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
