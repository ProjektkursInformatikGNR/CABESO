using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CABESO.Views.Counter
{
    public class CounterController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public CounterController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
    }
}