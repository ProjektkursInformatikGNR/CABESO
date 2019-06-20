using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Bearbeitung eines Allergens
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class EditAllergenModel : PageModel
    {
        /// <summary>
        /// Das zu bearbeitende Allergen
        /// </summary>
        public static Allergen CurrentAllergen { get; private set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="EditAllergenModel"/>.
        /// </summary>
        /// <param name="context"></param>
        public EditAllergenModel(ApplicationDbContext context)
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
            /// Die Beschreibung des zu bearbeitenden Allergens
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte die Beschreibung an.")]
            [Display(Name = "Beschreibung")]
            public string Description { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er initialisiert das zu bearbeitende Allergen <see cref="CurrentAllergen"/> anhand der ID.</para>
        /// </summary>
        /// <param name="id">
        /// Die ID des zu bearbeitenden Allergens
        /// </param>
        public void OnGet(int id)
        {
            CurrentAllergen = _context.Allergens.Find(id);
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Bearbeiten"-Buttons).<para>
        /// Er bearbeitet auf Grundlage der eingegebenen Informationen das gegebenene Allergen.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                CurrentAllergen.Description = Input.Description;
                _context.Allergens.Update(CurrentAllergen);
                _context.SaveChanges();
                return LocalRedirect("~/Counter/Allergens");
            }
            return Page();
        }
    }
}