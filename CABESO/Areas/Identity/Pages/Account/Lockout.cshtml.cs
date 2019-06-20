using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Benachrichtigung eines Benutzers über seinen Lockout
    /// </summary>
    [AllowAnonymous]
    public class LockoutModel : PageModel
    {
    }
}