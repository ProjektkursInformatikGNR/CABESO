using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Bearbeitung eines Benutzers
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class EditUserModel : PageModel
    {
        /// <summary>
        /// Der zu bearbeitende Benutzer
        /// </summary>
        public static IdentityUser CurrentUser { get; private set; }

        /// <summary>
        /// Der Indikator, ob der zu bearbeitende Benutzer der einzige Admin ist und, wenn dem so ist, diese Berechtigungsstufe behalten muss
        /// </summary>
        public bool StuckAsAdmin { get; private set; }

        /// <summary>
        /// Die zur Verfügung stehenden Schulklassen
        /// </summary>
        public Form[] Forms { get; private set; }

        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private readonly SignInManager<IdentityUser> _signInManager; //Der Manager der Anmeldeverwaltung
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="EditUserModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="signInManager">
        /// Die Anmeldeverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public EditUserModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert die Eigenschaften <see cref="CurrentUser"/>, <see cref="StuckAsAdmin"/> sowie <see cref="Forms"/>.</para>
        /// </summary>
        /// <param name="id">
        /// Die ID des zu bearbeitenden Benutzers
        /// </param>
        /// <returns>
        /// Syntaktisch wird ein <see cref="Task"/> zurückgegeben, wegen des <c>await</c>-Schlüsselworts beim Aufruf entspricht dieser semantisch jedoch <c>void</c>.
        /// </returns>
        public async Task OnGet(string id)
        {
            CurrentUser = Database.GetUserById(id);
            StuckAsAdmin = (await _userManager.GetUsersInRoleAsync(Resources.Admin)).Count <= 1;
            Forms = _context.GetFormsSelect();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Bearbeiten"-Buttons).<para>
        /// Er bearbeitet auf Grundlage der eingegebenen Informationen den gegebenen Benutzer.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                bool refresh = CurrentUser.Id.Equals(User.GetIdentityUser().Id);
                CurrentUser.Email = Input.Email;
                CurrentUser.NormalizedEmail = Input.Email.ToUpper();
                CurrentUser.UserName = Input.Email;
                CurrentUser.NormalizedUserName = Input.Email.ToUpper();
                _context.Users.Update(CurrentUser);
                _context.UserRoles.RemoveRange(_context.UserRoles.Where(userRole => userRole.UserId.Equals(CurrentUser.Id)));
                _context.UserRoles.Add(new IdentityUserRole<string>()
                {
                    UserId = CurrentUser.Id,
                    RoleId = _context.Roles.FirstOrDefault(role => role.Name.Equals(Input.Role)).Id
                });
                if (Input.Admin)
                    _context.UserRoles.Add(new IdentityUserRole<string>()
                    {
                        UserId = CurrentUser.Id,
                        RoleId = _context.Roles.FirstOrDefault(role => role.Name.Equals(Resources.Admin)).Id
                    });
                _context.SaveChanges();
                CurrentUser.SetFormId(Input.Role.Equals(Resources.Student) ? Input.FormId : null);
                if (refresh)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(CurrentUser, false);
                }
                return LocalRedirect("~/Admin/Index");
            }
            return Page();
        }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            /// <summary>
            /// Die E-Mail-Adresse des zu bearbeitenden Benutzers (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "E-Mail-Adresse")]
            public string Email { get; set; }

            /// <summary>
            /// Die Rolle (Schüler*in, Lehrer*in oder Mitarbeiter*in) des zu bearbeitenden Benutzers (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Rolle")]
            public string Role { get; set; }

            /// <summary>
            /// Die ID der Schulklasse des zu bearbeitenden Benutzers (optional)
            /// </summary>
            [Display(Name = "Klasse")]
            public int? FormId { get; set; }

            /// <summary>
            /// Die Berechtigungsstufe (Admin oder Benutzer) des zu bearbeitenden Benutzers (erforderlich)
            /// </summary>
            [Required]
            [Display(Name = "Administrator: ")]
            public bool Admin { get; set; }
        }
    }
}