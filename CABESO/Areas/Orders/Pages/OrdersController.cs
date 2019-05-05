using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Orders
{
    public class OrdersController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult PlaceOrder()
        {
            return View();
        }

        [Authorize]
        public IActionResult OrderHistory()
        {
            return View();
        }
    }
}