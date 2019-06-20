using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Anmeldung eines Benutzers mit seinen Kontodaten
    /// </summary>
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private readonly SignInManager<IdentityUser> _signInManager; //Der Manager der Anmeldeverwaltung

        /// <summary>
        /// Erzeugt ein neues <see cref="LoginModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="signInManager">
        /// Die Anmeldeverwaltungsinstanz per Dependency Injection
        /// </param>
        public LoginModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
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
        /// Die URL, zu der nach Beendigung der Anmeldung zurückgekehrt werden soll
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            /// <summary>
            /// Die E-Mail-Adresse zur Anmeldung (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = Program.ErrorMessage)]
            [Display(Name = "wwschool-Adresse (Der Namensteil genügt.)")]
            public string Email { get; set; }

            /// <summary>
            /// Das Passwort zur Anmeldung (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = Program.ErrorMessage)]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }

            /// <summary>
            /// Der Indikator, ob die Anmeldedaten als Cookie lokal gespeichert werden sollen (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Angemeldet bleiben?")]
            public bool RememberMe { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er loggt ggf. den aktuellen Benutzer aus und initialisiert die Eigenschaft <see cref="ReturnUrl"/>.</para>
        /// </summary>
        /// <param name="returnUrl">
        /// Die URL, zu der nach Beendigung der Anmeldung zurückgekehrt werden soll
        /// </param>
        /// <returns>
        /// Syntaktisch wird ein <see cref="Task"/> zurückgegeben, wegen des <c>await</c>-Schlüsselworts beim Aufruf entspricht dieser semantisch jedoch <c>void</c>.
        /// </returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Anmelden"-Buttons).<para>
        /// Er loggt den Benutzer mit den gegebenen Kontodaten ein und zeigt ggf. Fehlermeldungen an.</para>
        /// </summary>
        /// <param name="returnUrl">
        /// Die URL, zu der nach Beendigung der Anmeldung zurückgekehrt werden soll
        /// </param>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                if (!Input.Email.Contains('@'))
                    Input.Email += "@gnr.wwschool.de";

                SignInResult result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                    return LocalRedirect(returnUrl);
                if (result.IsLockedOut)
                    return RedirectToPage("./Lockout");
                IdentityUser user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null && !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    ModelState.AddModelError(string.Empty, "Bitte bestätige deine E-Mail-Adresse!");
                    return Page();
                }
                ModelState.AddModelError(string.Empty, "Die Anmeldedaten sind ungültig.");
            }

            return Page();
        }
    }
}
