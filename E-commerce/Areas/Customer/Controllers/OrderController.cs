using E_commerce.Data.Repository.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
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
            cartViewModel.orderHeader.ApplicationUser = _unitOfWork.User.FirstOrDefault(u => u.Id == userId);

            cartViewModel.orderHeader.Name = cartViewModel.orderHeader.ApplicationUser.Name;
            cartViewModel.orderHeader.PhoneNumber = cartViewModel.orderHeader.ApplicationUser.PhoneNumber;
            cartViewModel.orderHeader.StreetAddress = cartViewModel.orderHeader.ApplicationUser.Street;
            cartViewModel.orderHeader.City = cartViewModel.orderHeader.ApplicationUser.City;
            cartViewModel.orderHeader.State = cartViewModel.orderHeader.ApplicationUser.State;
            cartViewModel.orderHeader.PostalCode = cartViewModel.orderHeader.ApplicationUser.PostalCode;



            foreach (var cart in cartViewModel.shoppingCarts)
            {
                cartViewModel.orderHeader.OrderTotal += (cart.Product.Price * cart.Quantity);
            }
            return View(cartViewModel);
        }
    }
}
