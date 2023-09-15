using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Models;
using MyWebApplication.Data;
using MyWebApplication.Data.Infrastructure.IRepository;
using MyWebApplication.Data.Migrations;
using MyWebApplication.Models.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace MyWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDBContext _context;
        private IUnitOfWork _unitofWork;
        private IWebHostEnvironment _hostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitOfWork;
            _hostingEnvironment = webHostEnvironment;
        }
        #region APICALL
        public IActionResult AllProducts()
        {
            var products = _unitofWork.Product.GetAll(includeProperties:"Category");
            return Json(new{Data=products});
        }
        #endregion
        public IActionResult Index()
        {
            ProductVM productVM = new ProductVM();
            productVM.products = _unitofWork.Product.GetAll();
            return View(productVM);
        }
     
        [HttpGet]
        public ActionResult CreateUpdate(int? id)
        {
            
            ProductVM productVM = new ProductVM()
            {
                product = new(),
                Categories = _unitofWork.Category.GetAll().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }),
            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                var product=_unitofWork.Product.GetT(p=>p.Id == id);
                productVM.product = product;
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(productVM);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUpdate(ProductVM vm,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                if(file!=null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    fileName=Guid.NewGuid().ToString()+"-"+file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    if(vm.product.ImageUrl!=null)
                    {
                        var OldImagePath = Path.Combine(_hostingEnvironment.WebRootPath,vm.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }
                    using(var fileStream= new FileStream(filePath,FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    vm.product.ImageUrl = @"\ProductImage/"+fileName;
                }
                if (vm.product.Id == 0)
                {
                    _unitofWork.Product.Add(vm.product);
                    TempData["success"] = "Product Created Sucessfully!!!";
                }
                else
                {
                    _unitofWork.Product.Update(vm.product);
                    TempData["success"] = "Product Updated Sucessfully!!!";
                }
                _unitofWork.Save();
            }
            return RedirectToAction("Index");
        }
        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var product = _unitofWork.Product.GetT(c => c.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(product);
        //}
        #region DeleteAPICALL
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitofWork.Product.GetT(c => c.Id == id);
            if (product == null)
            {
                return Json(new {success=false,message="Error in Fetching Data"});
            }
            else
            {
                if (product.ImageUrl != null)
                {
                    var OldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }
                }
                _unitofWork.Product.Delete(product);
                _unitofWork.Save();

                return Json(new { success = true, message = "Product Deleted!!!" });
            }
        }
        #endregion
    }
}
