using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StepItUp.Models;

namespace StepItUp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        // use ViewData dictionary to set a value in the controller & display the value in the view
        ViewData["Timestamp"] = DateTime.Now;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
