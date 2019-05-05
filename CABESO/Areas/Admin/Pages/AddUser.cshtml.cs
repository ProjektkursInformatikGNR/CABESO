using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AddUserModel : PageModel
    {
        public Form[] Forms { get; private set; }

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;

        public AddUserModel(UserManager<IdentityUser> userManager, ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte die E-Mail-Adresse an.")]
            [EmailAddress(ErrorMessage = "Gib bitte eine gültige E-Mail-Adresse an.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            public string Role { get; set; }

            public int FormId { get; set; }
        }

        public void OnGet()
        {
            Forms = _context.Forms.OrderBy(form => form.ToString()).ToArray();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            OnGet();

            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                byte[] password = new byte[32];
                RandomNumberGenerator.Fill(password);
                string pwd = Convert.ToBase64String(password);
                IdentityResult result = await _userManager.CreateAsync(user, pwd);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created with random password.");

                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string callbackUrl = $"http://{HttpContext.Request.Host}/Identity/Account/CreatePassword?userId={user.Id}&code={Uri.EscapeDataString(code)}";

                    if (!Program.SendMail(
                        user.Email,
                        "Erstanmeldung",
                        $"Du wurdest zur Teilnahme an der Cafeteria-Bestellsoftware (CABESO) eingeladen. Klicke <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier</a> zur Erstanmeldung.",
                        new Name(user.Email, Input.Role.Equals(Resources.Student))) ||
                        !Program.MailValid(Input.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Die angegebene E-Mail-Adresse konnte nicht erreicht werden.");
                        await _userManager.DeleteAsync(user);
                        return Page();
                    }

                    await _userManager.AddToRoleAsync(user, Input.Role);
                    if (Input.Role.Equals(Resources.Student))
                        user.SetFormId(Input.FormId);

                    await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
                    return RedirectToPage();
                }

                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}