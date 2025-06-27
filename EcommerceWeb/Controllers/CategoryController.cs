using EcommerceWeb.Data;
using EcommerceWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;
    public CategoryController(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }
    // GET
    public IActionResult Index()
    {
        List<Category> categories = _db.Categories.ToList();
        return View(categories);
    }
}