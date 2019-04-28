using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        public Client Client { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("./AccessDenied");
            }

            Client = Client.Create(_userManager, User);
            return Page();
        }
    }
}