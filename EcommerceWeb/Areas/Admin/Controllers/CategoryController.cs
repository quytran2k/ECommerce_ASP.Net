using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers;

[Area("Admin")]

public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    // GET
    public IActionResult Index()
    {
        List<Category> categories = _unitOfWork.Category.GetAll().ToList();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The DisplayOrder cannot be the same as the Name");
        }

        if (obj.Name == "test")
        {
            ModelState.AddModelError("", "Test is invalid value");
        }

        if (!ModelState.IsValid) return View();
        _unitOfWork.Category.Add(obj);
        _unitOfWork.Save();
        TempData["Message"] = "Category added successfully";
        return RedirectToAction("Index");
    }
    
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Category? categoryFromDb = _unitOfWork.Category.Get(category => category.Id == id);
        // Find only search primary key, but first or default can help you to search other attribute
        // which is not primary key and return null if not found
        // Category? categoryFromDb1 = _db.Categories.FirstOrDefault(category => category.Name == "test");
        
        // Where can help you to have multiple filter if we have
        // Category? categoryFromDb1 = _db.Categories.Where(category => category.Id==id).FirstOrDefault();
        
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (obj.Name == "test")
        {
            ModelState.AddModelError("", "Test is invalid value");
        }

        if (!ModelState.IsValid) return View();
        _unitOfWork.Category.Update(obj);
        _unitOfWork.Save();
        TempData["Message"] = "Category edited successfully";
        return RedirectToAction("Index");
    }
     
    public IActionResult Delete(int? id)
    {
        if (id is null or 0)
        {
            return NotFound();
        }
        Category? categoryFromDb = _unitOfWork.Category.Get(category => category.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category? categoryFromDb = _unitOfWork.Category.Get(category => category.Id == id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        _unitOfWork.Category.Remove(categoryFromDb);
        _unitOfWork.Save();
        TempData["Message"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }
}