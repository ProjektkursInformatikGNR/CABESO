using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung

        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return RedirectToPage("/Index");

            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound($"Unable to load user with ID '{userId}'.");

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}