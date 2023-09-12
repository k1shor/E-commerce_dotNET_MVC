using E_commerce.Data.Repository.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using Utils;

namespace E_commerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles =StaticData.ROLE_CUSTOMER)]
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
        [ActionName("Index")]
        public IActionResult PlaceOrder()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartViewModel.shoppingCarts = _unitOfWork.ShoppingCart.FindAll(u => u.ApplicationUserID == userId,
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

            //stripe logic
            var domain = "https://localhost:7083/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/order/OrderConfirmation?id={cartViewModel.orderHeader.Id}",
                CancelUrl = domain + "customer/order/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in cartViewModel.shoppingCarts)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100), // $20.50 => 2050
                        Currency = "npr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title,
                            Description = item.Product.ImageUrl,
                            Images = new List<string> {item.Product.ImageUrl}
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.OrderHeader.UpdateStripePaymentID(cartViewModel.orderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);



            /*return RedirectToAction(nameof(OrderConfirmation), new
            {
                id = cartViewModel.orderHeader.Id
            });*/
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.FirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                _unitOfWork.OrderHeader.UpdateStatus(id, StaticData.Order_Status_CONFIRMED, StaticData.Payment_Status_CONFIRMED);
                _unitOfWork.Save();
            }

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .FindAll(u => u.ApplicationUserID == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.DeleteRange(shoppingCarts);
            _unitOfWork.Save();
            HttpContext.Session.Clear();
            return View(id);
        }

        
    }
}
