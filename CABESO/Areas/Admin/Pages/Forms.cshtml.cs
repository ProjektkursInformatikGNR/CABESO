using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Übersicht der Schulklassen
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class FormsModel : PageModel
    {
        /// <summary>
        /// Die zur Verfügung stehenden Schulklassen
        /// </summary>
        public Form[] Forms { get; private set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Namen der Schulklasse
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// Der zu suchende Ausdruck
        /// </summary>
        public string SearchKeyWord { get; set; }

        /// <summary>
        /// Erzeugt ein neues <see cref="FormsModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public FormsModel(ApplicationDbContext context)
        {
            Forms = context.Forms.ToArray();
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
                Forms = Forms.Search(search, form => form.ToString()).ToArray();
            Sort = string.IsNullOrEmpty(sortOrder) ? "!" : "";

            IOrderedEnumerable<Form> forms = Forms.OrderBy(user => 0);
            switch (sortOrder)
            {
                case "!":
                    forms = forms.OrderByDescending(form => form.GetGrade().Year);
                    break;
                case "":
                    forms = forms.OrderBy(form => form.GetGrade().Year);
                    break;
            }
            Forms = forms.ToArray();
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
            /// Der zu suchende Ausdruck (optional) 
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
            return RedirectToAction("Forms", "Admin", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}