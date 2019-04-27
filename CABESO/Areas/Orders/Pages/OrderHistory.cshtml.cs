using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.HistoricOrders.Pages
{
    public class OrdersHistoryModel : PageModel
    {
        public string ProductNameSort { get; set; }
        public string PricePerProductSort { get; set; }
        public string TotalPriceSort { get; set; }
        public string NumberSort { get; set; }
        public string OrderTimeSort { get; set; }
        public string CollectionTimeSort { get; set; }
        public HistoricOrder[] HistoricOrders { get; set; }

        public string SearchKeyWord { get; set; }

        private readonly UserManager<IdentityUser> _userManager;

        public OrdersHistoryModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public void OnGet(string sortOrder, string search)
        {
            SearchKeyWord = search ?? string.Empty;
            HistoricOrders = HistoricOrder.Enumerate().Where(historicOrder => historicOrder.Client.Equals(Client.Create(_userManager, User))).ToArray();
            if (!string.IsNullOrEmpty(SearchKeyWord))
                HistoricOrders = HistoricOrders.Where(historicOrder => Program.Matches(historicOrder.Product.Name, SearchKeyWord)).ToArray();

            OrderTimeSort = string.IsNullOrEmpty(sortOrder) ? "!ot" : "";
            ProductNameSort = sortOrder == "p" ? "!p" : "p";
            PricePerProductSort = sortOrder == "ppp" ? "!ppp" : "ppp";
            TotalPriceSort = sortOrder == "tp" ? "!tp" : "tp";
            NumberSort = sortOrder == "n" ? "!n" : "n";
            CollectionTimeSort = sortOrder == "ct" ? "!ct" : "ct";

            IOrderedEnumerable<HistoricOrder> historicOrders = HistoricOrders.OrderBy(historicOrder => 0);

            switch (sortOrder)
            {
                case "p":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.Product.Name);
                    break;
                case "!p":
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.Product.Name);
                    break;
                case "ppp":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.Product.Price);
                    break;
                case "!ppp":
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.Product.Price);
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
                case "!ot":
                    historicOrders = historicOrders.OrderBy(historicOrder => historicOrder.OrderTime);
                    break;
                default:
                    historicOrders = historicOrders.OrderByDescending(historicOrder => historicOrder.OrderTime);
                    break;
            }

            HistoricOrders = historicOrders.ThenByDescending(historicOrder => historicOrder.CollectionTime).ToArray();
        }

        [BindProperty]
        public InputModel Input { get; set; }

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