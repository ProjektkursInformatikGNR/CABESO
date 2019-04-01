using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        public readonly object[][] Forms;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger)
            //IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //_emailSender = emailSender;

            Forms = Database.SqlQuery("Forms", null, "Name", "Id");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public bool Confirmed { get; set; }

        public string Role;

        public class InputModel
        {
            [Required]
            [StringLength(10, ErrorMessage = "Der Code muss genau 10 Zeichen lang sein.", MinimumLength = 10)]
            [DataType(DataType.Text)]
            [Display(Name = "Code")]
            public string Code { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "E-Mail Adresse")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} und höchstens {1} Zeichen lang sein.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Passwort bestätigen")]
            [Compare("Password", ErrorMessage = "Die beiden Passwörter stimmen nicht überein.")]
            public string ConfirmPassword { get; set; }

            public string Form { get; set; }
        }

        public void OnGet(string code = null, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Confirmed = CodeValid(code, out Role);
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                if (!Confirmed)
                    RedirectToPage(new { code = Input.Code });

                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    await _userManager.AddToRoleAsync(user, Role);
                    Database.SqlExecute($"UPDATE [dbo].[AspNetUsers] SET {(Role.Equals("Student") ? $"[Form]={Input.Form}, " : "")}[EmailConfirmed]='True' WHERE [Id]='{user.Id}';");
                    Database.SqlExecute($"DELETE FROM [dbo].[Codes] WHERE [Code]='{Input.Code}';");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            return Page();
        }

        private bool CodeValid(string code, out string role)
        {
            role = null;
            if (string.IsNullOrEmpty(code))
                return false;

            object[][] codes = Database.SqlQuery("Codes", $"[Code]='{Input.Code}'", "RoleId");

            if (codes.Length > 0)
            {
                role = codes[0][0].ToString();
                return true;
            }
            return false;
        }
    }
}