using CABESO.Data;
using CABESO.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zum Hinzufügen eines Benutzers
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AddUserModel : PageModel
    {
        /// <summary>
        /// Die zur Auswahl stehenden Schulklassen
        /// </summary>
        public Form[] Forms { get; private set; }

        private readonly UserManager<IdentityUser> _userManager; //Der Manager der Benutzerverwaltung
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="AddUserModel"/>.
        /// </summary>
        /// <param name="userManager">
        /// Die Benutzerverwaltungsinstanz per Dependency Injection
        /// </param>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public AddUserModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

		/// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        /// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
        /// </summary>
        /// <summary>
		/// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
		/// </summary>
		public class InputModel
        {
            /// <summary>
            /// Die E-Mail-Adresse des hinzuzufügenden Benutzers (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte die E-Mail-Adresse an.")]
            [EmailAddress(ErrorMessage = "Gib bitte eine gültige E-Mail-Adresse an.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            /// Die Rolle (Schüler*in, Lehrer*in oder Mitarbeiter*in) des hinzuzufügenden Benutzers (erforderlich)
            /// </summary>
            [Required]
            public string Role { get; set; }

            /// <summary>
            /// Die ID der Schulklasse des hinzuzufügenden Benutzers (optional)
            /// </summary>
            public int FormId { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert die Datenstruktur <see cref="Forms"/>.</para>
        /// </summary>
        public void OnGet()
        {
            Forms = _context.GetFormsSelect();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Hinzufügen"-Buttons).<para>
        /// Er erstellt auf Grundlage der eingegebenen Informationen einen neuen Benutzer.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            OnGet();

            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                byte[] password = new byte[32];
                RandomNumberGenerator.Fill(password);
                string pwd = Convert.ToBase64String(password);
                IdentityResult result = await _userManager.CreateAsync(user, pwd);
                if (result.Succeeded)
                {
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string callbackUrl = $"http://{HttpContext.Request.Host}/Identity/Account/CreatePassword?userId={user.Id}&code={Uri.EscapeDataString(code)}";

                    if (!Program.SendMail(
                        user.Email,
                        new Name(user.Email, Input.Role.Equals(Resources.Student)),
                        "Erstanmeldung",
                        "Du wurdest zur Teilnahme an der Cafeteria-Bestellsoftware (CABESO) eingeladen. Klicke {0} zur Erstanmeldung.",
                        (callbackUrl, "hier")) ||
                        !Program.MailValid(Input.Email))
                    {
                        ModelState.AddModelError(string.Empty, "Die angegebene E-Mail-Adresse konnte nicht erreicht werden.");
                        await _userManager.DeleteAsync(user);
                        return Page();
                    }

                    await _userManager.AddToRoleAsync(user, Input.Role);
                    if (Input.Role.Equals(Resources.Student))
                        user.SetFormId(Input.FormId);

                    await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
                    return LocalRedirect("~/Admin/Index");
                }

                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}