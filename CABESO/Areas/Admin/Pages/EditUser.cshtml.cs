using CABESO.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    public class EditUserModel : PageModel
    {
        public static IdentityUser CurrentUser { get; private set; }
        public bool StuckAsAdmin { get; private set; }
        public Form[] Forms { get; private set; }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public EditUserModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task OnGet(string id)
        {
            CurrentUser = Database.GetUserById(id);
            StuckAsAdmin = (await _userManager.GetUsersInRoleAsync("Admin")).Count <= 1;
            Forms = _context.Forms.OrderBy(form => form.ToString()).ToArray();
        }

        public IActionResult OnPost()
        {
            CurrentUser.Email = Input.Email;
            CurrentUser.UserName = Input.Email;
            _context.Users.Update(CurrentUser);
            _context.UserRoles.RemoveRange(_context.UserRoles.Where(userRole => userRole.UserId.Equals(CurrentUser.Id)));
            _context.UserRoles.Add(new IdentityUserRole<string>()
            {
                UserId = CurrentUser.Id,
                RoleId = _context.Roles.FirstOrDefault(role => role.Name.Equals(Input.Role)).Id
            });
            if (Input.Admin)
                _context.UserRoles.Add(new IdentityUserRole<string>()
                {
                    UserId = CurrentUser.Id,
                    RoleId = Database.AdminId
                });
            _context.SaveChanges();
            CurrentUser.SetFormId(Input.Role.Equals("Student") ? Input.FormId : null);
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