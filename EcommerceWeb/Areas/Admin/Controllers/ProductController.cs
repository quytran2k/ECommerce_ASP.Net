using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    // GET
    public IActionResult Index()
    {
        List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return View(products);
    }

    public IActionResult Upsert(int? id)
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
        if (id is null or 0)
        {
            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.Product.Get(product => product.Id == id);
            return View(productVM);
        }
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM productVM, IFormFile? file)
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
        string wwwRootPath = _webHostEnvironment.WebRootPath;
        if (file != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productPath = Path.Combine(wwwRootPath, @"images/product");

            if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
            {
                // Delete old file
                string oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            if (!Directory.Exists(productPath))
            {
                Directory.CreateDirectory(productPath);
            }

            using (FileStream fs = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            productVM.Product.ImageUrl = @"/images/product/" + fileName;
        }

        if (productVM.Product.Id == 0)
        {
            _unitOfWork.Product.Add(productVM.Product);
        }
        else
        {
            _unitOfWork.Product.Update(productVM.Product);
        }
        _unitOfWork.Save();
        TempData["Message"] = $"Product {(productVM.Product.Id == 0 ? "created" : "updated")} successfully";
        return RedirectToAction("Index");
    }
    
    /*
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
    */

    #region API Calls

    [HttpGet]
    public IActionResult GetAll()
    {
        List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        return Json(new { data = products });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        Product? productToDelete = _unitOfWork.Product.Get(product => product.Id == id);
        if (productToDelete == null)
        {
            return Json( new { success=false, message = "Product not found" });
        }
        string oldImagePath = 
            Path.Combine(_webHostEnvironment.WebRootPath, 
                productToDelete.ImageUrl.TrimStart('/'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        _unitOfWork.Product.Remove(productToDelete);
        _unitOfWork.Save();
        return Json( new { success=true, message = "Product removed successfully" });
    }

    #endregion
}