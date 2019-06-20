using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Übersicht der Bestellungen
    /// </summary>
    [Authorize(Roles = "Admin,Employee")]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Namen des Bestellers bzw. der Bestellerin
        /// </summary>
        public string UserNameSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Produktbezeichnung
        /// </summary>
        public string ProductNameSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Gesamtpreis der Bestellung
        /// </summary>
        public string TotalPriceSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Menge der bestellten Produkte
        /// </summary>
        public string NumberSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Bestellzeit
        /// </summary>
        public string OrderTimeSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Abholzeit
        /// </summary>
        public string CollectionTimeSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf den Abholort
        /// </summary>
        public string CollectionPlaceSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Zubereitungszeit
        /// </summary>
        public string PreparationTimeSort { get; set; }

        /// <summary>
        /// Das Sortierverhalten der Tabelle in Bezug auf die Anmerkungen zur Bestellung
        /// </summary>
        public string NoteSort { get; set; }

        /// <summary>
        /// Die derzeit offenen Bestellungen
        /// </summary>
        public Order[] Orders { get; private set; }

        /// <summary>
        /// Der zu suchende Ausdruck
        /// </summary>
        public string SearchKeyWord { get; set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erzeugt ein neues <see cref="IndexModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per Dependency Injection
        /// </param>
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
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
        public void OnGet(string sortOrder, string search)
        {
            Orders = _context.Orders.ToArray();
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Orders = Orders.Search(search, order => order.Product.Name, order => order.User.GetName()).ToArray();

            OrderTimeSort = string.IsNullOrEmpty(sortOrder) ? "!ot" : "";
            UserNameSort = sortOrder == "un" ? "!un" : "un";
            ProductNameSort = sortOrder == "p" ? "!p" : "p";
            TotalPriceSort = sortOrder == "tp" ? "!tp" : "tp";
            NumberSort = sortOrder == "n" ? "!n" : "n";
            CollectionTimeSort = sortOrder == "ct" ? "!ct" : "ct";
            CollectionTimeSort = sortOrder == "cp" ? "!cp" : "cp";
            PreparationTimeSort = sortOrder == "pt" ? "!pt" : "pt";
            NoteSort = sortOrder == "no" ? "!no" : "no";

            IOrderedEnumerable<Order> orders = Orders.OrderBy(order => 0);

            switch (sortOrder)
            {
                case "un":
                    orders = orders.OrderBy(order => order.User.GetName());
                    break;
                case "!un":
                    orders = orders.OrderByDescending(order => order.User.GetName());
                    break;
                case "p":
                    orders = orders.OrderBy(order => order.Product.Name);
                    break;
                case "!p":
                    orders = orders.OrderByDescending(order => order.Product.Name);
                    break;
                case "tp":
                    orders = orders.OrderBy(order => order.Product.Price * order.Number);
                    break;
                case "!tp":
                    orders = orders.OrderByDescending(order => order.Product.Price * order.Number);
                    break;
                case "n":
                    orders = orders.OrderBy(order => order.Number);
                    break;
                case "!n":
                    orders = orders.OrderByDescending(order => order.Number);
                    break;
                case "pt":
                    orders = orders.OrderBy(order => order.PreparationTime);
                    break;
                case "!pt":
                    orders = orders.OrderByDescending(order => order.PreparationTime);
                    break;
                case "ct":
                    orders = orders.OrderBy(order => order.CollectionTime);
                    break;
                case "!ct":
                    orders = orders.OrderByDescending(order => order.CollectionTime);
                    break;
                case "cp":
                    orders = orders.OrderBy(order => order.CollectionPlace);
                    break;
                case "!cp":
                    orders = orders.OrderByDescending(order => order.CollectionPlace);
                    break;
                case "no":
                    orders = orders.OrderBy(order => order.Notes);
                    break;
                case "!no":
                    orders = orders.OrderByDescending(order => order.Notes);
                    break;
                case "!ot":
                    orders = orders.OrderBy(order => order.OrderTime);
                    break;
                default:
                    orders = orders.OrderByDescending(order => order.OrderTime);
                    break;
            }

            Orders = orders.ThenByDescending(order => order.CollectionTime).ToArray();
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
            return RedirectToAction("Index", "Counter", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}