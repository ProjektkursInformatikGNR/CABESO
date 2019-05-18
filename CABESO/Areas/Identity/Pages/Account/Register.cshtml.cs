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
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        public Form[] Forms { get; private set; }

        private static string _code;

        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(UserManager<IdentityUser> userManager, ILogger<RegisterModel> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            Forms = _context.GetFormsSelect();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }


        public static IdentityRole Role;

        public static bool Confirmed { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(10, ErrorMessage = "Der Code muss genau 10 Zeichen lang sein.", MinimumLength = 10)]
            [DataType(DataType.Text)]
            [Display(Name = "Code")]
            public string Code { get; set; }

            [Required]
            [EmailAddress]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@gnr\.wwschool\.de)$", ErrorMessage = "Bitte nutze deine wwschool-Mail zur Registrierung.")]
            [Display(Name = "wwschool-Adresse")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Passwörter müssen mindestens {2} und höchstens {1} Zeichen lang sein.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Passwort bestätigen")]
            [Compare("Password", ErrorMessage = "Die beiden Passwörter stimmen nicht überein.")]
            public string ConfirmPassword { get; set; }

            public int? FormId { get; set; }
        }

        public void OnGet(string code = null, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Confirmed = CodeValid(_code = code);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            if (!Confirmed)
                return RedirectToPage(new { code = Input.Code });

            ModelState.Remove("Input.Code");
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    string callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = await _userManager.GenerateEmailConfirmationTokenAsync(user) },
                        protocol: Request.Scheme);

                    if (!Program.SendMail(
                        Input.Email,
                        "E-Mail-Adresse bestätigen",
                        $"Bitte bestätige deine E-Mail-Adresse, indem du <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier</a> klickst.",
                        new Name(user.Email, Role.Name.Equals(Resources.Student))) ||
                        !Program.MailValid(Input.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Die angegebene E-Mail-Adresse konnte nicht erreicht werden.");
                        await _userManager.DeleteAsync(user);
                        return Page();
                    }

                    await _userManager.AddToRoleAsync(user, Role.Name);

                    if (_userManager.Users.Count() == 1)
                        await _userManager.AddToRoleAsync(user, Resources.Admin);

                    user.SetFormId(Input.FormId);
                    _context.Codes.Remove(_context.Codes.Find(_code));
                    _context.SaveChanges();

                    Program.Alert = "Bitte überprüfe dein E-Mail-Postfach und bestätige deine E-Mail-Adresse!";
                    return LocalRedirect(ReturnUrl);
                }

                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private bool CodeValid(string code)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            RegistrationCode regCode = _context.Codes.Find(code);

            if (regCode != null)
            {
                Role = regCode.Role;
                return true;
            }

            ModelState.AddModelError(string.Empty, "Dieser Code ist ungültig.");
            return false;
        }
    }
}