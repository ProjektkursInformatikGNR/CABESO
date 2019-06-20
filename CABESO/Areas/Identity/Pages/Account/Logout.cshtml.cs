using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Abmeldung des aktuellen Benutzers
    /// </summary>
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager; //Der Manager der Anmeldeverwaltung

        /// <summary>
        /// Erzeugt ein neues <see cref="LogoutModel"/>.
        /// </summary>
        /// <param name="signInManager">
        /// Die Anmeldeverwaltungsinstanz per Dependency Injection
        /// </param>
        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier automatisch durch Aufrufen der Razor Page).<para>
        /// Er loggt den aktuellen Benutzer aus.</para>
        /// </summary>
        /// <param name="returnUrl">
        /// Die URL, zu der nach Beendigung der Abmeldung zurückgekehrt werden soll
        /// </param>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
                return LocalRedirect(returnUrl);
            else
                return Page();
        }
    }
}