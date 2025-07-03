using System.Diagnostics;
using Ecommerce.Models;
using EcommerceWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Customer.Controllers;

[Area("Customer")]

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IGuidServices _guidServices;
    private readonly IGuidServices _guidServices2;

    public HomeController(ILogger<HomeController> logger, IGuidServices guidServices, IGuidServices guidServices2)
    {
        _logger = logger;
        _guidServices = guidServices;
        _guidServices2 = guidServices2;
    }

    public IActionResult Index()
    {
        Console.WriteLine($"Guid: {_guidServices.GetGuid()}");
        Console.WriteLine($"Guid2: {_guidServices2.GetGuid()}");
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}