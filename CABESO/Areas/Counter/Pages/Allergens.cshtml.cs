using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Übersicht der Allergene
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AllergensModel : PageModel
    {
        /// <summary>
        /// Die zur Verfügung stehenden Allergene
        /// </summary>
        public Allergen[] Allergens { get; private set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Bezeichnung des Allergens
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Der zu suchende Ausdruck
        /// </summary>
        public string SearchKeyWord { get; set; }

        /// <summary>
        /// Erzeugt ein neues <see cref="AllergensModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext
        /// </param>
        public AllergensModel(ApplicationDbContext context)
        {
            Allergens = context.Allergens.ToArray();
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, wenn die Weboberfläche angefordert wird.<para>
        /// Er konfiguriert gegebenenfalls die Tabelle gemäß der Suchanfrage.</para>
        /// </summary>
        /// <param name="search">
        /// Der zu suchende Ausdruck
        /// </param>
        /// <param name="sortOrder">
        /// Das Sortierverhalten
        /// </param>
        public void OnGet(string search, string sortOrder)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Allergens = Allergens.Search(search, allergen => allergen.ToString()).ToArray();
            Sort = string.IsNullOrEmpty(sortOrder) ? "!" : "";

            IOrderedEnumerable<Allergen> allergens = Allergens.OrderBy(allergen => 0);
            switch (sortOrder)
            {
                case "!":
                    allergens = allergens.OrderByDescending(allergen => allergen.Description);
                    break;
                default:
                    allergens = allergens.OrderBy(allergen => allergen.Description);
                    break;
            }
            Allergens = allergens.ToArray();
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
            /// Der zu suchende Ausdruck
            /// </summary>
            public string SearchKeyWord { get; set; }
        }

        /// <summary>
        /// Dieser Event Handler wird aufgerufen, sobald das "POST"-Event auslöst wird (hier durch Betätigung des "Suchen"-Buttons).<para>
        /// Er lädt die Weboberfläche neu, sodass der Suchanfrage Rechnung getragen werden kann.</para>
        /// </summary>
        /// <returns>
        /// Die Anweisung zum Neuladen der Oberfläche
        /// </returns>
        public IActionResult OnPost()
        {
            return RedirectToAction("Allergens", "Counter", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}