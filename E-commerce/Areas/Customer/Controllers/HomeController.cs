using E_commerce.Data.Repository.IRepository;
using E_commerce.Models;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Diagnostics;
using System.Security.Claims;
using Utils;

namespace E_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                HttpContext.Session.SetInt32(StaticData.SessionCart,
                _unitOfWork.ShoppingCart.FindAll(u => u.ApplicationUserID == claim.Value).Count());
            }

            IEnumerable<Product> products = _unitOfWork.Product.FindAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult ViewDetail(int productId)
        {
            /*Product product = _unitOfWork.Product.FirstOrDefault(u => u.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);*/
            ShoppingCart shoppingCart = new()
            {
                Product = _unitOfWork.Product.FirstOrDefault(u => u.ID == productId),
                Quantity = 1,
                ProductID = productId
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult ViewDetail(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            cart.ApplicationUserID = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.ProductID == cart.ProductID && u.ApplicationUserID==userId);
            if (cartFromDb != null)
            {
                cartFromDb.Quantity += cart.Quantity;
                TempData["success"] = "Quantity increased in cart";

            }
            else
            {
                _unitOfWork.ShoppingCart.Create(cart);
                TempData["success"] = "Item added to cart";
            }
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(StaticData.SessionCart,
               _unitOfWork.ShoppingCart.FindAll(u => u.ApplicationUserID == userId).Count());

            return RedirectToAction("Index");


        }
    }
}