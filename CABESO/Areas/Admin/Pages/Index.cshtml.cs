using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Admin.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Übersicht der Benutzer
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Die zur Verfügung stehende Benutzer
        /// </summary>
        public IdentityUser[] Users { get; private set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Vornamen des Benutzers
        /// </summary>
        public string FirstNameSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Nachnamen des Benutzers
        /// </summary>
        public string LastNameSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Rolle des Benutzers
        /// </summary>
        public string RoleSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Schulklasse des Benutzers
        /// </summary>
        public string FormSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Administratorenstatus des Benutzers
        /// </summary>
        public string AdminSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Mitarbeiterstatus des Benutzers
        /// </summary>
        public string EmployeeSort { get; set; }

        /// <summary>
        /// Der zu suchende Ausdruck
        /// </summary>
        public string SearchKeyWord { get; set; }

        /// <summary>
        /// Erzeugt ein neues <see cref="IndexModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public IndexModel(ApplicationDbContext context)
        {
            Users = context.Users.ToArray();
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
                Users = Users.Search(search, user => user.GetName(), user => user.GetForm().ToString()).ToArray();

            FirstNameSort = string.IsNullOrEmpty(sortOrder) ? "!fn" : "";
            LastNameSort = sortOrder == "ln" ? "!ln" : "ln";
            RoleSort = sortOrder == "r" ? "!r" : "r";
            FormSort = sortOrder == "f" ? "!f" : "f";
            AdminSort = sortOrder == "a" ? "!a" : "a";
            EmployeeSort = sortOrder == "e" ? "!e" : "e";

            IOrderedEnumerable<IdentityUser> users = Users.OrderBy(user => 0);
            switch (sortOrder)
            {
                case "!fn":
                    users = users.OrderByDescending(user => user.GetName().FirstName);
                    break;
                case "ln":
                    users = users.OrderBy(user => user.GetName().LastName);
                    break;
                case "!ln":
                    users = users.OrderByDescending(user => user.GetName().LastName);
                    break;
                case "r":
                    users = users.OrderBy(user => user.GetRole().GetDisplayName());
                    break;
                case "!r":
                    users = users.OrderByDescending(user => user.GetRole().GetDisplayName());
                    break;
                case "f":
                    users = users.OrderBy(user => user.GetForm().ToString());
                    break;
                case "!f":
                    users = users.OrderByDescending(user => user.GetForm().ToString());
                    break;
                case "a":
                    users = users.OrderBy(user => !user.IsAdmin());
                    break;
                case "!a":
                    users = users.OrderByDescending(user => !user.IsAdmin());
                    break;
                case "e":
                    users = users.OrderBy(user => !user.IsEmployee());
                    break;
                case "!e":
                    users = users.OrderByDescending(user => !user.IsEmployee());
                    break;
                default:
                    users = users.OrderBy(user => user.GetName().FirstName);
                    break;
            }

            Users = users.ThenBy(user => user.GetName().LastName).ToArray();
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
            return RedirectToAction("Index", "Admin", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}