using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Battleship.Models;

namespace Battleship.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("Menu");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Reset()
    {
        HttpContext.Session.Remove("_Game");
        HttpContext.Session.Remove("_TwoPlayerGame");
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
