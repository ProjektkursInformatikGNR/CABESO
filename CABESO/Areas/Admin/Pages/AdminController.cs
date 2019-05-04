using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CABESO.Views.Admin
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GenerateCodes()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);

            return LocalRedirect("~/Admin/Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditUser()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeactivateCode(string code)
        {
            Database.Context.Codes.Remove(RegistrationCode.GetCodeByCode(code));
            Database.Context.SaveChanges();
            return LocalRedirect("~/Admin/GenerateCodes");
        }
    }
}