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


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> objOrderHeaders = _unitOfWork.OrderHeader.FindAll(includeProperties: "ApplicationUser").ToList();
            return Json(new { data = objOrderHeaders });
        }


        #endregion
    }
}
