using CABESO.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung

        public ForgotPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    string callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { code = await _userManager.GeneratePasswordResetTokenAsync(user) },
                        protocol: Request.Scheme);
                    Program.SendMail(
                        Input.Email,
                        "Passwort zurücksetzen",
                        $"Bitte setze dein Passwort zurück, indem du <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>hier</a> klickst.",
                        new Name(user.Email, user.GetRole()?.Name.Equals(Resources.Student) ?? false));
                }

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}