using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        public IdentityUser CurrentUser { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return Redirect("/Identity/Account/AccessDenied");
            return Page();
        }
    }
}