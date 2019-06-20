using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Modellklasse der Razor Page zur Verifizierung einer neuen E-Mail-Adresse
    /// </summary>
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung

        /// <summary>
        /// Erzeugt ein neues <see cref="ConfirmEmailModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er verifiziert den benutzerspezifischen Bestätigungscode und gibt ggf. Fehlermeldungen aus.</para>
        /// </summary>
        /// <param name="userId">
        /// Die ID des Benutzers, dessen E-Mail-Adresse zu verifizieren ist.
        /// </param>
        /// <param name="code">
        /// Der intern generierte Bestätigungscode für die E-Mail-Adresse
        /// </param>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code)) //fehlerhafter bzw. unvollstädiger Bestätigungslink
                return RedirectToPage("/Index");

            IdentityUser user = await _userManager.FindByIdAsync(userId);
            if (user == null) //falsche Nutzer-ID
                return NotFound($"Unable to load user with ID '{userId}'.");

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}