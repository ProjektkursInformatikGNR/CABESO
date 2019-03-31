using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CABESO.Views.Admin
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
        public async Task<IActionResult> PromoteAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !id.Equals(UserEntity.GetUser(_userManager, User).Id))
                await _userManager.AddToRoleAsync(user, "Admin");

            return LocalRedirect("~/Admin/Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DegradeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !id.Equals(UserEntity.GetUser(_userManager, User).Id))
                await _userManager.RemoveFromRoleAsync(user, "Admin");

            return LocalRedirect("~/Admin/Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PromoteEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !id.Equals(UserEntity.GetUser(_userManager, User).Id))
                await _userManager.AddToRoleAsync(user, "Employee");

            return LocalRedirect("~/Admin/Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DegradeEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !id.Equals(UserEntity.GetUser(_userManager, User).Id))
                await _userManager.RemoveFromRoleAsync(user, "Employee");

            return LocalRedirect("~/Admin/Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeactivateCode(string id)
        {
            Database.SqlExecute($"DELETE FROM [dbo].[Codes] WHERE [Code]='{id}';");
            return LocalRedirect("~/Admin/GenerateCodes");
        }
    }
}