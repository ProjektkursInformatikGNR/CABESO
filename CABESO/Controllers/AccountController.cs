using Microsoft.AspNetCore.Mvc;

namespace CABESO.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return Redirect("/Identity/Account/Login");
        }
    }
}