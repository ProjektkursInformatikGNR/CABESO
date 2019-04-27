using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Kitchen
{
    public class KitchenController : Controller
    {
        [Authorize(Roles = "Employee,Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}