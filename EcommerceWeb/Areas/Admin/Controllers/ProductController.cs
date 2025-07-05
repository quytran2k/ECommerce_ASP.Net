using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceWeb.Areas.Admin.Controllers;

[Area("Admin")]

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GET
    public IActionResult Index()
    {
        List<Product> products = _unitOfWork.Product.GetAll().ToList();
        return View(products);
    }

    public IActionResult Create()
    {
        IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()
            }
        );
        // ViewBag.CategoryList = categoryList;
        ProductVM productVM = new()
        {
            CategoryList = categoryList,
            Product = new Product()
        };
        return View(productVM);
    }

    [HttpPost]
    public IActionResult Create(ProductVM productVM)
    {
        if (!ModelState.IsValid)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(category => new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }
            );
            productVM.CategoryList = categoryList;
            return View(productVM);
        }
        _unitOfWork.Product.Add(productVM.Product);
        _unitOfWork.Save();
        TempData["Message"] = "Product added successfully";
        return RedirectToAction("Index");
    }
    
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Product? productFromDb = _unitOfWork.Product.Get(product => product.Id == id);
        
        if (productFromDb == null)
        {
            return NotFound();
        }
        return View(productFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Product obj)
    {
        if (!ModelState.IsValid) return View();
        _unitOfWork.Product.Update(obj);
        _unitOfWork.Save();
        TempData["Message"] = "Product edited successfully";
        return RedirectToAction("Index");
    }
     
    public IActionResult Delete(int? id)
    {
        if (id is null or 0)
        {
            return NotFound();
        }
        Product? productFromDb = _unitOfWork.Product.Get(product => product.Id == id);
        if (productFromDb == null)
        {
            return NotFound();
        }
        return View(productFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Product? productFromDb = _unitOfWork.Product.Get(product => product.Id == id);
        if (productFromDb == null)
        {
            return NotFound();
        }
        _unitOfWork.Product.Remove(productFromDb);
        _unitOfWork.Save();
        TempData["Message"] = "Product deleted successfully";
        return RedirectToAction("Index");
    }
}