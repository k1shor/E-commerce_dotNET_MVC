using E_commerce.Data.Repository.IRepository;
using E_commerce.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            IEnumerable<Product> products = _unitOfWork.Product.FindAll("Category");
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
        public IActionResult ViewDetail(int id)
        {
            Product product = _unitOfWork.Product.FirstOrDefault(u => u.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}