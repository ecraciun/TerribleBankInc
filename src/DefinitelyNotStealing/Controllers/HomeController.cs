using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DefinitelyNotStealing.Data;
using DefinitelyNotStealing.Models;
using DefinitelyNotStealing.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DefinitelyNotStealing.Controllers;

public class HomeController : Controller
{
    private readonly AppDataContext _ctx;

    public HomeController(AppDataContext context)
    {
        _ctx = context;
    }

    public IActionResult Index()
    {
        var allCapturedGoodies = _ctx.Set<ExfiltratedData>().ToList();
        return View(allCapturedGoodies);
    }

    public IActionResult Prize()
    {
        return View();
    }

    public IActionResult Win()
    {
        return View();
    }
}
