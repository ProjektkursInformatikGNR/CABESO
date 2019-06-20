using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Anzeige der Kontoinfomationen des aktuellen Benutzers
    /// </summary>
    [Authorize]
    public partial class IndexModel : PageModel
    {
        /// <summary>
        /// Der aktuell angemeldete Benutzer
        /// </summary>
        public IdentityUser CurrentUser { get; private set; }

        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung

        /// <summary>
        /// Erzeugt ein neues <see cref="IndexModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        public IndexModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert ggf. die Eigenschaft <see cref="CurrentUser"/> und gibt andernfalls eine Zugriffsablehnung aus.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await _userManager.GetUserAsync(User);
            if (CurrentUser == null)
                return Redirect("/Identity/Account/AccessDenied");
            return Page();
        }
    }
}