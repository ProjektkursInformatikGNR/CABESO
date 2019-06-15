using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    [Authorize]
    public class OrderHistoryModel : PageModel
    {
        public string ProductNameSort { get; set; }
        public string TotalPriceSort { get; set; }
        public string NumberSort { get; set; }
        public string OrderTimeSort { get; set; }
        public string CollectionTimeSort { get; set; }
        public string CollectionPlaceSort { get; set; }
        public HistoricOrder[] HistoricOrders { get; set; }

        public string SearchKeyWord { get; set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        public OrderHistoryModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("OrderHistory", "Orders", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}