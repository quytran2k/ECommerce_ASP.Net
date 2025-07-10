using System.Diagnostics;
using System.Security.Claims;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using EcommerceWeb.Services;
using Microsoft.AspNetCore.Authorization;
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
        ShoppingCart cart = new ShoppingCart()
        {
            Product = _unitOfWork.Product.Get(product => product.Id == productId, includeProperties: "Category"),
            Count = 1,
            ProductId = productId
        };
        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        shoppingCart.ApplicationUserId = userId;

        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
            u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

        if(cartFromDb != null)
        {
            // Shopping cart already exists, update the count
            cartFromDb.Count += shoppingCart.Count;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
        }
        else
        {
            _unitOfWork.ShoppingCart.Add(shoppingCart);
        }

        _unitOfWork.Save();
        TempData["Message"] = "Cart update successfully";
        return RedirectToAction(nameof(Index));
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