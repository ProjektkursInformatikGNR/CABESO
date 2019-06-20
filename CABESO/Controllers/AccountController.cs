using Microsoft.AspNetCore.Mvc;

namespace CABESO.Controllers
{
    /// <summary>
    /// Der Controller der Subarea "Identity.Account" ist zuständig für die Ausführung der Actions.
    /// </summary>
    public class AccountController : Controller
    {
        /// <summary>
        /// Die Weiterleitung zur Razor Page "Login"
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        public IActionResult Login()
        {
            return Redirect("/Identity/Account/Login");
        }
    }
}