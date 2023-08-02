using E_commerce.Data.Repository.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils;

namespace E_commerce.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public CartViewModel cartViewModel { get; set; }
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
			cartViewModel.orderHeader.StreetAddress = cartViewModel.orderHeader.ApplicationUser.Street;
			cartViewModel.orderHeader.City = cartViewModel.orderHeader.ApplicationUser.City;
			cartViewModel.orderHeader.State = cartViewModel.orderHeader.ApplicationUser.State;
			cartViewModel.orderHeader.PostalCode = cartViewModel.orderHeader.ApplicationUser.PostalCode;
			cartViewModel.orderHeader.PhoneNumber = cartViewModel.orderHeader.ApplicationUser.PhoneNumber;



			foreach (var cart in cartViewModel.shoppingCarts)
			{
				cartViewModel.orderHeader.OrderTotal += (cart.Product.Price * cart.Quantity);
			}
			return View(cartViewModel);
		}
		[HttpPost]
		public IActionResult PlaceOrder()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			cartViewModel.shoppingCarts= _unitOfWork.ShoppingCart.FindAll(u => u.ApplicationUserID == userId,
				includeProperties: "Product");

			cartViewModel.orderHeader.OrderDate = System.DateTime.Now;
			cartViewModel.orderHeader.ApplicationUserId = userId;

			cartViewModel.orderHeader.ApplicationUser = _unitOfWork.User.FirstOrDefault(u => u.Id == userId);


			foreach (var cart in cartViewModel.shoppingCarts)
			{
				cartViewModel.orderHeader.OrderTotal += (cart.Product.Price * cart.Quantity);
			}
						
				//payment and order status
				cartViewModel.orderHeader.PaymentStatus = StaticData.Payment_Status_PENDING;
				cartViewModel.orderHeader.OrderStatus = StaticData.Order_Status_PENDING;
			
				
			_unitOfWork.OrderHeader.Create(cartViewModel.orderHeader);
			_unitOfWork.Save();
			foreach (var cart in cartViewModel.shoppingCarts)
			{
				OrderDetails orderDetail = new()
				{
					ProductId = cart.ProductID,
					OrderHeaderId = cartViewModel.orderHeader.Id,
					Price = cart.Product.Price,
					Count = cart.Quantity
				};
				_unitOfWork.OrderDetails.Create(orderDetail);
				_unitOfWork.Save();
			}


			return RedirectToAction(nameof(OrderConfirmation), new { id = cartViewModel.orderHeader.Id });
		}

		public IActionResult OrderConfirmation(int id)
		{
			return View(id);
		}
	}
}
