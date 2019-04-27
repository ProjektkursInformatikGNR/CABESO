using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Counter
{
    public class CounterController : Controller
    {
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Orders()
        {
            return View();
        }

        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Products()
        {
            return View();
        }
    }
}