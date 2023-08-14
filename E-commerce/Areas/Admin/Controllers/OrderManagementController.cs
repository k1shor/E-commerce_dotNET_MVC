using E_commerce.Data.Repository.IRepository;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            List<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.FindAll(includeProperties: "ApplicationUser").ToList();
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
