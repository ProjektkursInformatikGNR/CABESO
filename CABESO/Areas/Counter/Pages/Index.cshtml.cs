using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    [Authorize(Roles = "Admin,Employee")]
    public class IndexModel : PageModel
    {
        public string UserNameSort { get; set; }
        public string ProductNameSort { get; set; }
        public string TotalPriceSort { get; set; }
        public string NumberSort { get; set; }
        public string OrderTimeSort { get; set; }
        public string CollectionTimeSort { get; set; }
        public string CollectionPlaceSort { get; set; }
        public string PreparationTimeSort { get; set; }
        public string NoteSort { get; set; }
        public Order[] Orders { get; set; }

        public string SearchKeyWord { get; set; }

        private readonly ApplicationDbContext _context; //Das Vermittlungsobjekt der Datenbankanbindung

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

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
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("Index", "Counter", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}