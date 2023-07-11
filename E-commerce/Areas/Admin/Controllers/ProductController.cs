using E_commerce.Data;
using E_commerce.Data.Repository.IRepository;
using E_commerce.Models;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet]
        public IActionResult Index()
        {
            //            List<Product> categories = _db.Categories.ToList();
            //IEnumerable<Product> categories = _db.FindAll();
            IEnumerable<Product> products = _unitOfWork.Product.FindAll("Category");

            return View(products);
        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category
                .FindAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                });

            ProductViewModel productViewModel = new ProductViewModel()
            {
                categoryList = categoryList,
                Product = new Product()
            };

            if (id != null && id != 0)
            {
                productViewModel.Product = _unitOfWork.Product.FirstOrDefault(u => u.ID == id);
            }

            //ViewBag.categoryList = categoryList;
            //ViewData["categoryList"] = categoryList;
            return View(productViewModel);
        }
        [HttpPost]
        public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
        {
            //ModelState.AddModelError("Name", "Product is required");

            if (ModelState.IsValid)
            {
                productVM.Product.updatedAt = DateTime.Now.ToString();
                //_db.Categories.Add(product);
               


                string wwwRoot = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string filepath = Path.Combine(wwwRoot, @"images\products", filename);

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldImage = Path.Combine(wwwRoot, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }
                    }

                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + filename;
                }
                //_db.SaveChanges();

                if (productVM.Product.ID == 0)
                {
                    productVM.Product.createdAt = DateTime.Now.ToString();
                    _unitOfWork.Product.Create(productVM.Product);
                    TempData["success"] = "Product Added Successfully";
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product Updated Successfully";
                }


                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.categoryList = _unitOfWork.Category
                .FindAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                });
                return View(productVM);
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
            string wwwRoot = _webHostEnvironment.WebRootPath;
            //            Product product = _db.Categories.Find(id);
            Product product = _unitOfWork.Product.FirstOrDefault(x => x.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            var Image = Path.Combine(wwwRoot,product.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(Image))
            {
                System.IO.File.Delete(Image);
            }


            //_db.Categories.Remove(product);
            _unitOfWork.Product.Delete(product);
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";

            return RedirectToAction("Index");
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Product> products = _unitOfWork.Product.FindAll("Category");
            return Json(new { data= products});
        }


        #endregion

    }
}
