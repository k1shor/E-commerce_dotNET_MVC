using E_commerce.Data;
using E_commerce.Data.Repository.IRepository;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;


        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //            List<Product> categories = _db.Categories.ToList();
            //IEnumerable<Product> categories = _db.FindAll();
            IEnumerable<Product> categories = _unitOfWork.Product.FindAll();

            return View(categories);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            //ModelState.AddModelError("Name", "Product is required");
            
            if (ModelState.IsValid)
            {
                product.createdAt = DateTime.Now.ToString();
                product.updatedAt = DateTime.Now.ToString();
                //_db.Categories.Add(product);
                _unitOfWork.Product.Create(product);
                //_db.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Product Added Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            //Product product = _db.Categories.FirstOrDefault(x => x.ID == id); 
            Product product = _unitOfWork.Product.FirstOrDefault(x => x.ID == id);
            return View(product);
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                product.updatedAt = DateTime.Now.ToString();
                _unitOfWork.Product.Update(product);
                //                _db.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            //            Product product = _db.Categories.Find(id);
            Product product = _unitOfWork.Product.FirstOrDefault(x => x.ID == id);
            return View(product);
        }

        [HttpPost]
        [ActionName("DeleteProduct")]
        public IActionResult Delete(int id)
        {
            //            Product product = _db.Categories.Find(id);
            Product product = _unitOfWork.Product.FirstOrDefault(x => x.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            //_db.Categories.Remove(product);
            _unitOfWork.Product.Delete(product);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";

            return RedirectToAction("Index");
        }

    }
}
