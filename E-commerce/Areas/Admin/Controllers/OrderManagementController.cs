using E_commerce.Data.Repository.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils;

namespace E_commerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
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
            OrderVM orderObj = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.FirstOrDefault(u => u.Id == orderId, "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetails.FindAll(u => u.OrderHeaderId == orderId, "Product").ToList()
            };
            return View(orderObj);
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateOrder(OrderVM orderObj)
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
            return RedirectToAction(nameof(OrderDetails), new { orderId = orderHeader.Id});
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
