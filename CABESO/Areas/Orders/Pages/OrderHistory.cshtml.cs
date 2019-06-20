using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur benutzerspezifischen Bestellhistorie
    /// </summary>
    [Authorize]
    public class OrderHistoryModel : PageModel
    {
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
        /// Die bisher archivierten Bestellungen
        /// </summary>
        public HistoricOrder[] HistoricOrders { get; set; }

        /// <summary>
        /// Der zu suchende Ausdruck
        /// </summary>
        public string SearchKeyWord { get; set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        /// <summary>
        /// Erezugt ein neues <see cref="OrderHistoryModel"/>.
        /// </summary>
        /// <param name="context">
        /// Der Datenbankkontext per DependencyInjection
        /// </param>
        public OrderHistoryModel(ApplicationDbContext context)
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
            SearchKeyWord = search ?? string.Empty;
            HistoricOrders = _context.HistoricOrders.Where(historicOrder => historicOrder.User.Id.Equals(User.GetIdentityUser().Id)).ToArray();
            if (!string.IsNullOrEmpty(SearchKeyWord))
                HistoricOrders = HistoricOrders.Search(search, historicOrder => historicOrder.Product.Name, historicOrder => historicOrder.User.GetName()).ToArray();

            OrderTimeSort = string.IsNullOrEmpty(sortOrder) ? "!ot" : "";
            ProductNameSort = sortOrder == "p" ? "!p" : "p";
            TotalPriceSort = sortOrder == "tp" ? "!tp" : "tp";
            NumberSort = sortOrder == "n" ? "!n" : "n";
            CollectionTimeSort = sortOrder == "ct" ? "!ct" : "ct";
            CollectionPlaceSort = sortOrder == "cp" ? "!cp" : "cp";

            IOrderedEnumerable<HistoricOrder> historicOrders = HistoricOrders.OrderBy(historicOrder => 0);

            switch (sortOrder)
            {
                case "p":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.Product.Name);
                    break;
                case "!p":
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.Product.Name);
                    break;
                case "tp":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.Product.Price * historicOrder.Number);
                    break;
                case "!tp":
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.Product.Price * historicOrder.Number);
                    break;
                case "n":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.Number);
                    break;
                case "!n":
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.Number);
                    break;
                case "ct":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.CollectionTime);
                    break;
                case "!ct":
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.CollectionTime);
                    break;
                case "cp":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.CollectionPlace);
                    break;
                case "!cp":
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.CollectionPlace);
                    break;
                case "!ot":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.OrderTime);
                    break;
                default:
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.OrderTime);
                    break;
            }

            HistoricOrders = historicOrders.ThenByDescending(historicOrder => historicOrder.CollectionTime).ToArray();
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
            return RedirectToAction("OrderHistory", "Orders", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}