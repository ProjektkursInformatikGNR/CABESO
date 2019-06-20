using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace CABESO.Areas.Identity.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Anzeige der Details eines internen Fehlers
    /// </summary>
    [AllowAnonymous]
    public class ErrorModel : PageModel
    {
        /// <summary>
        /// Die Request-ID des aufgeworfenen Fehlers
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Der Indikator, ob die Request-ID in der Weboberfläche angezeigt werden soll
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert die Eigenschaft <see cref="RequestId"/>.</para>
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}