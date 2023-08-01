using E_commerce.Data.Repository.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class cartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartViewModel CartViewModel;
        public cartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            CartViewModel cartViewModel = new()
            {
                shoppingCarts = _unitOfWork.ShoppingCart.FindAll(u => u.ApplicationUserID == userId, "Product"),
                orderHeader = new()
            };
            foreach (var cartObj in cartViewModel.shoppingCarts)
            {
                cartViewModel.orderHeader.OrderTotal += cartObj.Product.Price * cartObj.Quantity;
            }
            return View(cartViewModel);
        }

        [HttpPost]
        public IActionResult decrease(int cartId)
        {
            ShoppingCart cartObj = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.ID == cartId);
            if (cartObj.Quantity <= 1)
            {
                _unitOfWork.ShoppingCart.Delete(cartObj);
            }
            else
            {
                cartObj.Quantity--;
            }
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Increase(int cartId)
        {
            ShoppingCart cartObj = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.ID == cartId);

            cartObj.Quantity++;
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Remove(int cartId)
        {
            ShoppingCart cartObj = _unitOfWork.ShoppingCart.FirstOrDefault(u => u.ID == cartId);

            _unitOfWork.ShoppingCart.Delete(cartObj);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
