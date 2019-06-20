using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Mitteilung der erfolgreichen Zurücksetzung des Benutzerpassworts
    /// </summary>
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
    }
}