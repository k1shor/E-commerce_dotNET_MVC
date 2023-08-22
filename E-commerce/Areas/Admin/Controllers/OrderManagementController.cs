using E_commerce.Data.Repository.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using Utils;

namespace E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderObj { get; set; }
        public OrderManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderDetails(int orderId)
        {
            orderObj = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.FirstOrDefault(u => u.Id == orderId, "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetails.FindAll(u => u.OrderHeaderId == orderId, "Product").ToList()
            };
            return View(orderObj);
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.FirstOrDefault(u => u.Id == orderObj.OrderHeader.Id);
            orderHeader.Name = orderObj.OrderHeader.Name;
            orderHeader.PhoneNumber = orderObj.OrderHeader.PhoneNumber;
            orderHeader.StreetAddress = orderObj.OrderHeader.StreetAddress;
            orderHeader.City = orderObj.OrderHeader.City;
            orderHeader.State = orderObj.OrderHeader.State;
            orderHeader.PostalCode = orderObj.OrderHeader.PostalCode;
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(OrderDetails), new { orderId = orderHeader.Id });
        }
        [HttpPost]
        [Authorize(Roles = StaticData.ROLE_ADMIN)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(orderObj.OrderHeader.Id, StaticData.Order_Status_PROCESSING);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(OrderDetails), new { orderId = orderObj.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = StaticData.ROLE_ADMIN)]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.FirstOrDefault(u => u.Id == orderObj.OrderHeader.Id);
            /* carrier and tracking number can be added in model and updated while shipping*/
            orderHeader.OrderStatus = StaticData.Order_Status_SHIPPED;
            orderHeader.ShippingDate = DateTime.Now;
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction(nameof(OrderDetails), new { orderId = orderObj.OrderHeader.Id });
        }
        [HttpPost]
        [Authorize(Roles = StaticData.ROLE_ADMIN)]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeader.FirstOrDefault(u => u.Id == orderObj.OrderHeader.Id);

            if (orderHeader.PaymentStatus == StaticData.Payment_Status_CONFIRMED)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);
            }

            _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, StaticData.Order_Status_CANCELLED, StaticData.Payment_Status_REFUNDED);
            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(OrderDetails), new { orderId = orderObj.OrderHeader.Id });

        }


        #region API CALLS

        [HttpGet]
        [Authorize]
        public IActionResult GetAll(string status)
        {
            List<OrderHeader> objOrderHeaders;
            if (User.IsInRole(StaticData.ROLE_ADMIN))
            {
                objOrderHeaders = _unitOfWork.OrderHeader.FindAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                objOrderHeaders = _unitOfWork.OrderHeader.FindAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser").ToList();

            }
            switch (status)
            {
                case "PENDING":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticData.Order_Status_PENDING).ToList();
                    break;
                case "PROCESSING":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticData.Order_Status_PROCESSING).ToList();
                    break;
                case "COMPLETED":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticData.Order_Status_COMPLETED).ToList();
                    break;
                case "CONFIRMED":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticData.Order_Status_CONFIRMED).ToList();
                    break;
                case "CANCELLED":
                    objOrderHeaders = objOrderHeaders.Where(u => u.OrderStatus == StaticData.Order_Status_CANCELLED).ToList();
                    break;

                default:
                    break;

            }
            return Json(new { data = objOrderHeaders });
        }


        #endregion
    }
}
