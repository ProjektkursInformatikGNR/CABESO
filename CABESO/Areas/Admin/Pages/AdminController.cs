using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CABESO.Views.Admin
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null && !user.Id.Equals(User.GetIdentityUser().Id))
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
            _context.Codes.Remove(_context.Codes.Find(code));
            _context.SaveChanges();
            return LocalRedirect("~/Admin/GenerateCodes");
        }
    }
}