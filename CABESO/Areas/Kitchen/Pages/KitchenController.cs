using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Kitchen
{
    public class KitchenController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public KitchenController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
    }
}