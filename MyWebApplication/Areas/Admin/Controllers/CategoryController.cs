using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Models;
using MyWebApplication.Data;
using MyWebApplication.Data.Infrastructure.IRepository;
using MyWebApplication.Data.Migrations;
using MyWebApplication.Models.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDBContext _context;
        private IUnitOfWork _unitofWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }
        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.categories = _unitofWork.Category.GetAll();
            return View(categoryVM);
        }
        [HttpGet]
        public ActionResult CreateUpdate(int? id)
        {
            CategoryVM categoryVM = new CategoryVM();
          
           
            if (id == null || id == 0)
            {
                return View(categoryVM);
            }
            else
            {
                var editCategory = _unitofWork.Category.GetT(c => c.Id == id);
                categoryVM.category = editCategory;
                if (editCategory == null)
                {
                 return NotFound();
                }
                else
                {
                    return View(categoryVM);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.category.Id == 0)
                {

                    _unitofWork.Category.Add(vm.category);
                    TempData["success"] = "Category Created Sucessfully!!!";
                   
                }
                else
                {
                    _unitofWork.Category.Update(vm.category);
                    TempData["success"] = "Category Updated Sucessfully!!!";
                }
                _unitofWork.Save();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitofWork.Category.GetT(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteData(int? id)
        {
            var category = _unitofWork.Category.GetT(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitofWork.Category.Delete(category);
            _unitofWork.Save();
            TempData["success"] = "Category Deleted Sucessfully!!!";
            return RedirectToAction("Index");
        }
    }
}
