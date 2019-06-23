using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CABESO.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Registrierung eines neuen Benutzers
    /// </summary>
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        /// <summary>
        /// Die zur Verfügung stehenden Schulklassen
        /// </summary>
        public Form[] Forms { get; private set; }

        private static string _code; //Der Registrierungscode

        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="RegisterModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public RegisterModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            Forms = _context.GetFormsSelect();
        }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Die URL, zu der nach Beendigung der Registrierung zurückgekehrt werden soll
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// Die Rolle, der der neue Benutzer zugeordnet werden soll
        /// </summary>
        public static IdentityRole Role;

        /// <summary>
        /// Der Indikator, ob der Registrierungsvorgang bereits durch Eingabe des Registrierungscode eingeleitet wurde
        /// </summary>
        public static bool Confirmed { get; set; }

        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            /// <summary>
            /// Der zehnstellige, alphanumerische Registrierungscode (erforderlich)
            /// </summary>
            [Required]
            [StringLength(10, ErrorMessage = "Der Code muss genau 10 Zeichen lang sein.", MinimumLength = 10)]
            [DataType(DataType.Text)]
            [Display(Name = "Code")]
            public string Code { get; set; }

            /// <summary>
            /// Die E-Mail-Adresse des zu registrierenden Benutzers (erforderlich)
            /// </summary>
            [Required]
            [EmailAddress]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@gnr\.wwschool\.de)$", ErrorMessage = "Bitte nutze deine wwschool-Mail zur Registrierung.")]
            [Display(Name = "wwschool-Adresse")]
            public string Email { get; set; }

            /// <summary>
            /// Das Passwort des zu registrierenden Benutzers (erforderlich)
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "Passwörter müssen mindestens {2} und höchstens {1} Zeichen lang sein.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }

            /// <summary>
            /// Das neue Passwort zur Bestätigung (erforderlich)
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Passwort bestätigen")]
            [Compare("Password", ErrorMessage = "Die beiden Passwörter stimmen nicht überein.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            /// Die ID der Schulklasse, der der zu registrierende Benutzer zugeordnet werden soll
            /// </summary>
            public int? FormId { get; set; }

            /// <summary>
            /// Der Indikator, ob die Allgemeinen Geschäftsbedingungen akzeptiert wurden
            /// </summary>
            [Required]
            public bool AgbAccepted { get; set; }

            /// <summary>
            /// Der Indikator, ob die Datenschutzerklärung akzeptiert wurde
            /// </summary>
            [Required]
            public bool PrivacyAccepted { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert die Eigenschaften <see cref="ReturnUrl"/> sowie <see cref="Confirmed"/>.</para>
        /// </summary>
        /// <param name="code">
        /// Der zehnstellige, alphanumerische Registrierungscode
        /// </param>
        /// <param name="returnUrl">
        /// Die URL, zu der nach Beendigung der Registrierung zurückgekehrt werden soll
        /// </param>
        public void OnGet(string code = null, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Confirmed = CodeValid(_code = code);
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Registrieren"-Buttons).<para>
        /// Er verifiziert die Kontodaten, registriert im Erfolgsfall den neuen Benutzer und sendet eine E-Mail zur Bestätigung der E-Mail-Adresse.</para>
        /// </summary>
        /// <param name="returnUrl">
        /// Die URL, zu der nach Beendigung der Registrierung zurückgekehrt werden soll
        /// </param>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            if (!Confirmed) //noch kein Registrierungscode eingegeben
                return RedirectToPage(new { code = Input.Code });

            ModelState.Remove("Input.Code");
            if (ModelState.IsValid && Input.AgbAccepted && Input.PrivacyAccepted)
            {
                IdentityUser user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    string callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = await _userManager.GenerateEmailConfirmationTokenAsync(user) },
                        protocol: Request.Scheme);

                    if (!Program.SendMail(
                        Input.Email,
                        new Name(user.Email, Role.Name.Equals(Resources.Student)),
                        "E-Mail-Adresse bestätigen",
                        "Bitte bestätige deine E-Mail-Adresse, indem du {0} klickst.",
                        (callbackUrl, "hier")) ||
                        !Program.MailValid(Input.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Die angegebene E-Mail-Adresse konnte nicht erreicht werden.");
                        await _userManager.DeleteAsync(user);
                        return Page();
                    }

                    await _userManager.AddToRoleAsync(user, Role.Name);

                    if (_userManager.Users.Count() == 1) //erster Benutzer muss Admin sein
                        await _userManager.AddToRoleAsync(user, Resources.Admin);

                    user.SetFormId(Input.FormId);
                    _context.Codes.Remove(_context.Codes.Find(_code));
                    _context.SaveChanges();

                    Program.Alert = "Bitte überprüfe dein E-Mail-Postfach und bestätige deine E-Mail-Adresse!";
                    return LocalRedirect(ReturnUrl);
                }

                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        /// <summary>
        /// Überprüft den eingegebenen Registrerungscode anhand der Datenbank und legt im Erfolgsfall die verknüpfte Rolle fest.
        /// </summary>
        /// <param name="code">
        /// Der zu verifizierende Registrierungscode
        /// </param>
        /// <returns>
        /// Gibt <c>true</c> wieder, wenn der Code gültig (also in der Datenbank aufzufinden) ist, sonst <c>false</c>.
        /// </returns>
        private bool CodeValid(string code)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            RegistrationCode regCode = _context.Codes.Find(code);

            if (regCode != null)
            {
                Role = regCode.Role;
                return true;
            }

            ModelState.AddModelError(string.Empty, "Dieser Code ist ungültig.");
            return false;
        }
    }
}