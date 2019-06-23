using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur manuellen Änderung des Benutzerpassworts
    /// </summary>
    [Authorize]
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private readonly SignInManager<IdentityUser> _signInManager; //Der Manager der Anmeldeverwaltung

        /// <summary>
        /// Erzeugt ein neues <see cref="ChangePasswordModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="signInManager">
        /// Die Anmeldeverwaltungsinstanz per Dependency Injection
        /// </param>
        public ChangePasswordModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Die grüne Statusmeldung (wird zur Mitteilung der erfolgreichen Änerung des Benutzerpassworts benutzt)
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            /// <summary>
            /// Das alte, zu ersetzende Benutzerpasswort (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = Program.ErrorMessage)]
            [DataType(DataType.Password)]
            [Display(Name = "das alte Passwort")]
            public string OldPassword { get; set; }

            /// <summary>
            /// Das neue Benutzerpasswort (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = Program.ErrorMessage)]
            [StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} und höchstens {1} Zeichen lang sein.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "das neue Passwort")]
            public string NewPassword { get; set; }

            /// <summary>
            /// Das neue Passwort zur Bestätigung (erforderlich)
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Neues Passwort bestätigen")]
            [Compare("NewPassword", ErrorMessage = "Das neue Passwort stimmt nicht mit der Bestätigung überein.")]
            public string ConfirmPassword { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er überprüft, ob aktuell ein gültiger Benutzer angemeldet ist.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnGetAsync()
        {
            IdentityUser user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            return Page();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Passwort ändern"-Buttons).<para>
        /// Er ändert das Passwort des aktuellen Benutzers und gibt eine Rückmeldung (entweder Statusmeldung oder Fehlermeldung).</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            IdentityUser user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            IdentityResult changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (IdentityError error in changePasswordResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Dein Passwort wurde erfolgreich geändert.";
            return RedirectToPage();
        }
    }
}