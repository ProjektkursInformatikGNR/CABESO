using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Generierung von Registrierungscodes
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class GenerateCodesModel : PageModel
    {
        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public GenerateInputModel GenerateInput { get; set; }

        /// <summary>
		/// Ein Hilfsobjekt, das die Eingabeinformationen der Weboberfläche zwischenspeichert.
		/// </summary>
		[BindProperty]
        public DeactivateInputModel DeactivateInput { get; set; }

        /// <summary>
        /// Die derezit aktiven Registrierungscodes
        /// </summary>
        public RegistrationCode[] Codes { get; private set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="GenerateCodesModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext der Dependency Injection
        /// </param>
        public GenerateCodesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
        /// </summary>
        public class GenerateInputModel
        {
            /// <summary>
            /// Die Anzahl der zu generierenden Codes.
            /// </summary>
            [Required]
            [Display(Name = "Anzahl")]
            public int Number { get; set; }

            /// <summary>
            /// Die Rolle (Schüler*in, Lehrer*in oder Mitarbeiter*in) der neuen Benutzer
            /// </summary>
            [Required]
            [Display(Name = "Rolle")]
            public string Role { get; set; }
        }

        /// <summary>
        /// Eine Datenstruktur zur Zwischenspeicherung der Eingabeinformationen
        /// </summary>
        public class DeactivateInputModel
        {
            [Required]
            [Display(Name = "Altersgrenze")]
            public DateTime Limit { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert die Datenstruktur <see cref="Codes"/>.</para>
        /// </summary>
        public void OnGet()
        {
            Codes = _context.Codes.OrderByDescending(code => code.CreationTime).ToArray();
        }
    }
}