using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Controllers
{
    /// <summary>
    /// Der Controller der Area "Admin" ist zuständig für die Ausführung der Actions.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt einen neuen <see cref="AdminController"/>.
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        public AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page "Index"
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page "GenerateCodes"
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize(Roles = "Admin")]
        public IActionResult GenerateCodes()
        {
            return View();
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page "AddUser"
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize(Roles = "Admin")]
        public IActionResult AddUser()
        {
            return View();
        }

        /// <summary>
        /// Entfernt einen gegebenen Benutzer aus der Datenbank.
        /// </summary>
        /// <param name="id">Die ID des zu entfernenden Benutzers</param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Benutzerübersicht
        /// </returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null && !user.Id.Equals(User.GetIdentityUser().Id))
                await _userManager.DeleteAsync(user);
            return LocalRedirect("~/Admin/Index");
        }

        /// <summary>
        /// Die Weiterleitung zur Razor Page "EditUser"
        /// </summary>
        /// <returns>
        /// Die Anweisung der Weiterleitung
        /// </returns>
        [Authorize(Roles = "Admin")]
        public IActionResult EditUser()
        {
            return View();
        }

        /// <summary>
        /// Deaktiviert einen <see cref="RegistrationCode"/>, indem dieser aus der Datenbank entfernt wird.
        /// </summary>
        /// <param name="code">
        /// Die alpha-numerische Zeichenkette des zu deaktivierenden Codes
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Codegenerierung
        /// </returns>
        [Authorize(Roles = "Admin")]
        public IActionResult DeactivateCode(string code)
        {
            _context.Codes.Remove(_context.Codes.Find(code));
            _context.SaveChanges();
            return LocalRedirect("~/Admin/GenerateCodes");
        }

        /// <summary>
        /// Entfernt eine <see cref="Form"/> aus der Datenbank.
        /// </summary>
        /// <param name="id">Die ID der zu entfernenden Schulklasse</param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Klassenübersicht
        /// </returns>
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveForm(int id)
        {
            Form form = _context.Forms.Find(id);
            _context.Forms.Remove(form);
            _context.SaveChanges();
            return LocalRedirect("~/Admin/Forms");
        }

        /// <summary>
        /// Generiert zufällig neue Registrierungscodes für die gegebene Rolle.
        /// </summary>
        /// <param name="number">
        /// Die Anzahl der zu generierenden Codes
        /// </param>
        /// <param name="role">
        /// Die Rolle der neuen Benutzer
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Codegenerierung
        /// </returns>
        [HttpPost]
        public IActionResult Generate(int number, string role)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int length = 10;
            for (int i = 0; i < number; i++)
            {
                string code = new string(Enumerable.Repeat(chars, length).Select(s => s[new Random().Next(s.Length)]).ToArray());
                _context.Codes.Add(new RegistrationCode() { Code = code, CreationTime = DateTime.UtcNow, Role = _context.Roles.FirstOrDefault(r => r.Name.Equals(role)) });
            }
            _context.SaveChanges();
            return LocalRedirect("~/Admin/GenerateCodes");
        }

        /// <summary>
        /// Deaktiviert obsolete Codes, indem diese aus der Datenbank entfernt werden.
        /// </summary>
        /// <param name="limit">
        /// Codes, die vor oder an diesem Tag generiert wurden, sind zu deaktivieren.
        /// </param>
        /// <returns>
        /// Die Anweisung zur Rückkehr zur Codegenerierung
        /// </returns>
        [HttpPost]
        public IActionResult DeactivateOldCodes(DateTime limit)
        {
            foreach (RegistrationCode code in _context.Codes)
                if (code.CreationTime.Date <= limit)
                    _context.Codes.Remove(code);
            _context.SaveChanges();
            return LocalRedirect("~/Admin/GenerateCodes");
        }
    }
}