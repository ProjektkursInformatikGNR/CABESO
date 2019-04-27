using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Orders
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PlaceOrder()
        {
            return View();
        }
        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}