using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Bearbeitung einer Schulklasse
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class EditFormModel : PageModel
    {
        /// <summary>
        /// Die zu bearbeitende Schulklasse
        /// </summary>
        public static Form CurrentForm { get; private set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <seealso cref="EditFormModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public EditFormModel(ApplicationDbContext context)
        {
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
		public class InputModel
        {
            /// <summary>
            /// Der Jahrgang der zu bearbeitenden Schulklasse (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte den Jahrgang an.")]
            [Display(Name = "Derzeitiger Jahrgang")]
            public int Year { get; set; }

            /// <summary>
            /// Der Klassenzug der zu bearbeitenden Schulklasse (optional)
            /// </summary>
            [Display(Name = "Klassenzug")]
            public string Stream { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert die zu bearbeitende Schulklasse <seealso cref="CurrentForm"/> anhand der ID.</para>
        /// </summary>
        /// <param name="id">
        /// Die ID der zu bearbeitenden Schulklasse
        /// </param>
        public void OnGet(int id)
        {
            CurrentForm = _context.Forms.Find(id);
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Bearbeiten"-Buttons).<para>
        /// Er bearbeitet auf Grundlage der eingegebenen Informationen die gegebene Schulklasse.</para>
        /// </summary>
        /// <returns>
        /// Ein <seealso cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                CurrentForm.Enrolment = ((Form.Grade)Input.Year).Enrolment;
                CurrentForm.Stream = string.IsNullOrEmpty(Input.Stream) ? "" : Input.Stream;
                _context.Forms.Update(CurrentForm);
                _context.SaveChanges();
                return LocalRedirect("~/Admin/Forms");
            }
            return Page();
        }
    }
}