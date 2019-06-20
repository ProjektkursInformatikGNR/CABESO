using CABESO.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Versendung einer E-Mail zur Zurücksetzung des Benutzerpassworts
    /// </summary>
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung

        /// <summary>
        /// Erzeugt ein neues <see cref="ForgotPasswordModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
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
            /// <summary>
            /// Die E-Mail-Adresse des Benutzers, der sein Passwort zurückzusetzen wünscht
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Senden"-Buttons).<para>
        /// Er verifiziert die E-Mail-Adresse und sendet im Erfolgsfall eine E-Mail mit einem Zurücksetzungslink.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
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
                        new Name(user.Email, user.GetRole()?.Name.Equals(Resources.Student) ?? false),
                        "Passwort zurücksetzen",
                        "Bitte setze dein Passwort zurück, indem du {0} klickst.",
                        (callbackUrl, "hier"));
                }

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}