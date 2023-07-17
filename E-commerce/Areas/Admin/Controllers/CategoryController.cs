using E_commerce.Data;
using E_commerce.Data.Repository.IRepository;
using E_commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.ROLE_ADMIN)]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;


        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //            List<Category> categories = _db.Categories.ToList();
            //IEnumerable<Category> categories = _db.FindAll();
            IEnumerable<Category> categories = _unitOfWork.Category.FindAll();

            return View(categories);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            //ModelState.AddModelError("Name", "Category is required");
            if (category.Name == "test")
            {
                ModelState.AddModelError("", "Invalid category name.");
            }
            if (ModelState.IsValid)
            {
                category.createdAt = DateTime.Now.ToString();
                category.updatedAt = DateTime.Now.ToString();
                //_db.Categories.Add(category);
                _unitOfWork.Category.Create(category);
                //_db.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Category Added Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            //Category category = _db.Categories.FirstOrDefault(x => x.ID == id); 
            Category category = _unitOfWork.Category.FirstOrDefault(x => x.ID == id);
            return View(category);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                category.updatedAt = DateTime.Now.ToString();
                _unitOfWork.Category.Update(category);
                //                _db.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            //            Category category = _db.Categories.Find(id);
            Category category = _unitOfWork.Category.FirstOrDefault(x => x.ID == id);
            return View(category);
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        public IActionResult Delete(int id)
        {
            //            Category category = _db.Categories.Find(id);
            Category category = _unitOfWork.Category.FirstOrDefault(x => x.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            //_db.Categories.Remove(category);
            _unitOfWork.Category.Delete(category);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";

            return RedirectToAction("Index");
        }

    }
}
