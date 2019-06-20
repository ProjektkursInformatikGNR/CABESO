using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zum Hinzufügen eines Allergens
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class AddAllergenModel : PageModel
    {
        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="AddAllergenModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public AddAllergenModel(ApplicationDbContext context)
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
            /// Die Beschreibung des hinzuzufügenden Allergens (erforderlich)
            /// </summary>
            [Required(AllowEmptyStrings = false, ErrorMessage = "Gib bitte die Beschreibung an.")]
            [Display(Name = "Beschreibung")]
            public string Description { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Hinzufügen"-Buttons).<para>
        /// Er erstellt auf Grundlage der eingegebenen Informationen ein neues Allergen.</para>
        /// </summary>
        /// <returns>
        /// Ein <see cref="IActionResult"/>, das bestimmt, wie nach Behandlung des Event vorgegangen werden soll.
        /// </returns>
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Allergen allergen = new Allergen()
                {
                    Description = Input.Description
                };
                _context.Allergens.Add(allergen);
                _context.SaveChanges();
                return LocalRedirect("~/Counter/Allergens");
            }
            return Page();
        }
    }
}