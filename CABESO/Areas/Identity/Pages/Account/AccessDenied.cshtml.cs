using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Anzeige einer fehlenden Zugriffsberechtigung
    /// </summary>
    [AllowAnonymous]
    public class AccessDeniedModel : PageModel
    {
    }
}