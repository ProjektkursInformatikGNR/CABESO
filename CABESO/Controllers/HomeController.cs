using CABESO.Data;
using CABESO.Models;
using CABESO.Properties;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace CABESO.Controllers
{
    /// <summary>
    /// Der Controller der Viewssektion "Home" ist zuständig für die Ausführung der Actions.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Erzeugt einen neuen <see cref="HomeController"/>.
        /// </summary>
        /// <param name="roleManager">
        /// Die Rollenverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        /// <param name="signInManager">
        /// Die Anmeldeverwaltungsinstanz per Dependency Injection
        /// </param>
        public HomeController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            foreach (string role in new[] { Resources.Admin, Resources.Teacher, Resources.Student, Resources.Employee }) //Initialisierung der Rollen
                if (!roleManager.RoleExistsAsync(role).Result)
                    while (!roleManager.CreateAsync(new IdentityRole(role)).Result.Succeeded) ;

            if (context.Users.Count() == 0) //Workaround für fehlerhafte Auto-Anmeldungen
                signInManager.SignOutAsync();
        }

        /// <summary>
        /// Die Weiterleitung zum View "Index" (Startseite)
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Die Weiterleitung zum View "PageNotFound" (meist durch einen Error 404 ausgelöst)
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Route("error/404")]
        public IActionResult PageNotFound()
        {
            return View();
        }

        /// <summary>
        /// Die Weiterleitung zum View "Error" (durch eine unbehandelte Ausnahme ausgelöst)
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}