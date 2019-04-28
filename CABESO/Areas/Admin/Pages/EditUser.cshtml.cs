using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    public class EditUserModel : PageModel
    {
        public static Client ClientToEdit { get; private set; }
        public bool StuckAsAdmin { get; private set; }

        private readonly UserManager<IdentityUser> _userManager;

        public EditUserModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task OnGet(string id)
        {
            ClientToEdit = Client.Create(id);
            StuckAsAdmin = (await _userManager.GetUsersInRoleAsync("Admin")).Count <= 1;
        }

        public async Task<IActionResult> OnPost()
        {
            IdentityUser user = ClientToEdit.ToIdentityUser(_userManager);
            await _userManager.SetEmailAsync(user, Input.Email);
            await _userManager.SetUserNameAsync(user, Input.Email);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRoleAsync(user, Input.Role);
            if (Input.Admin)
                await _userManager.AddToRoleAsync(user, "Admin");
            Database.Update("AspNetUsers", $"[Id] = '{user.Id}'", new { Form = Input.Role.Equals("Student") && Input.FormId.HasValue ? (int?) (Input.FormId.Value) : null });

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