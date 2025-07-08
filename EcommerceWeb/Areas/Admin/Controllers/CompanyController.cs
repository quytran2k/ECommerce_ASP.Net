using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CompanyController
        public ActionResult Index()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }

        // GET: CompanyController/Upsert/

        public IActionResult Upsert(int? id)
        {
            if (id is null or 0)
            {
                return View(new Company());
            }
            else
            {
                Company companyObj = _unitOfWork.Company.Get(company => company.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (!ModelState.IsValid)
            {
                return View(CompanyObj);
            }

            if (CompanyObj.Id == 0)
            {
                _unitOfWork.Company.Add(CompanyObj);
            }
            else
            {
                _unitOfWork.Company.Update(CompanyObj);
            }
            _unitOfWork.Save();
            TempData["Message"] = $"Company {(CompanyObj.Id == 0 ? "created" : "updated")} successfully";
            return RedirectToAction("Index");
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company? companyToDelete = _unitOfWork.Company.Get(Company => Company.Id == id);
            if (companyToDelete == null)
            {
                return Json(new { success = false, message = "Company not found" });
            }
            _unitOfWork.Company.Remove(companyToDelete);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company removed successfully" });
        }

        #endregion
    }
}
