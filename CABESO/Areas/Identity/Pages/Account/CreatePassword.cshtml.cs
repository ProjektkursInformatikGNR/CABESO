using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Erstellung eines Passworts für einen neuen Benutzer
    /// </summary>
    [AllowAnonymous]
    public class CreatePasswordModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager; //Der Manager der Anmeldeverwaltung
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private static string _userId; //Die ID des neuen Benutzers

        /// <summary>
        /// Erzeugt ein neues <see cref="CreatePasswordModel"/>.
        /// </summary>
        /// <param name="signInManager">
        /// Die Anmeldeverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        public CreatePasswordModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
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
            /// Das neue Passwort für den Benutzer (erforderlich)
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} und höchstens {1} Zeichen lang sein.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "neue Passwort")]
            public string Password { get; set; }

            /// <summary>
            /// Das neue Passwort zur Bestätigung (erforderlich)
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare(nameof(Password), ErrorMessage = "Die Passwörter stimmen nicht überein.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// (versteckt) Der Passworterstellungscode per E-Mail
            /// </summary>
            public string Code { get; set; }

            /// <summary>
            /// (versteckt) Die ID des neuen Benutzers
            /// </summary>
            public string UserId { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er verifiziert die Benutzer-ID und initialisiert im Erfolgsfall die URL-Daten.</para>
        /// </summary>
        /// <param name="userId">
        /// Die ID des Benutzers, dessen Passwort festzulegen ist.
        /// </param>
        /// <param name="code">
        /// Der intern generierte Passworterstellungscode für den Benutzer
        /// </param>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnGet(string userId = null, string code = null)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(userId) || await _userManager.FindByIdAsync(_userId = userId) == null) //fehlerhafter bzw. unvollstädiger Passworterstellungslink
                return RedirectToPage("./AccessDenied");
            else
            {
                Input = new InputModel
                {
                    Code = code,
                    UserId = userId
                };
                return Page();
            }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Passwort einrichten"-Buttons).<para>
        /// Er verifiziert den Passworterstellungscode und legt im Erfolgsfall das neue Passwort fest.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            IdentityUser user = await _userManager.FindByIdAsync(_userId);
            if (user == null)
                return Page();

            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            IdentityResult result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return LocalRedirect("~/");
            }
            else
                return RedirectToPage("./AccessDenied");
        }
    }
}