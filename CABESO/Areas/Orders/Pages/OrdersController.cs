using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Orders
{
    public class OrdersController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}