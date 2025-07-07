using System.Diagnostics;
using Ecommerce.DataAccess.Repository.IRepository;
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
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IGuidServices guidServices, IGuidServices guidServices2, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _guidServices = guidServices;
        _guidServices2 = guidServices2;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        Console.WriteLine($"Guid: {_guidServices.GetGuid()}");
        Console.WriteLine($"Guid2: {_guidServices2.GetGuid()}");
        IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
        return View(products);
    }

    public IActionResult Details(int productId)
    {
        Product product = _unitOfWork.Product.Get(product => product.Id == productId, "Category");
        return View(product);
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