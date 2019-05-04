using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    public class EditUserModel : PageModel
    {
        public static IdentityUser CurrentUser { get; private set; }
        public bool StuckAsAdmin { get; private set; }

        private readonly UserManager<IdentityUser> _userManager;

        public EditUserModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGet(string id)
        {
            CurrentUser = Database.GetUserById(id);
            StuckAsAdmin = (await _userManager.GetUsersInRoleAsync("Admin")).Count <= 1;
        }

        public async Task<IActionResult> OnPost()
        {
            CurrentUser.Email = Input.Email;
            CurrentUser.UserName = Input.Email;
            Database.Context.Users.Update(CurrentUser);
            await _userManager.RemoveFromRolesAsync(CurrentUser, await _userManager.GetRolesAsync(CurrentUser));
            await _userManager.AddToRoleAsync(CurrentUser, Input.Role);
            if (Input.Admin)
                await _userManager.AddToRoleAsync(CurrentUser, "Admin");
            CurrentUser.SetFormId(Input.FormId);
            Database.Context.SaveChanges();

            return LocalRedirect("~/Admin/Index");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "E-Mail-Adresse")]
            public string Email { get; set; }

            [Display(Name = "Rolle")]
            public string Role { get; set; }

            [Display(Name = "Klasse")]
            public int? FormId { get; set; }

            [Display(Name = "Administrator: ")]
            public bool Admin { get; set; }
        }
    }
}