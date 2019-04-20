using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CABESO.Areas.Counter.Pages
{
    [Authorize(Roles = "Admin,Employee")]
    public class OrdersModel : PageModel
    {
        public string OrdererNameSort { get; set; }
        public string ProductNameSort { get; set; }
        public string PricePerProductSort { get; set; }
        public string TotalPriceSort { get; set; }
        public string CountSort { get; set; }
        public string TimeSort { get; set; }
        public Order[] Orders { get; set; }

        public string SearchKeyWord { get; set; }

        public OrdersModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            Orders = EnumerateOrders();
        }

        private Order[] EnumerateOrders()
        {
            return Array.ConvertAll(Database.SqlQuery("Products", null, "Name", "Price"), p => new Order() { ProductName = p[0].ToString(), PricePerProduct = (decimal)p[1] });
        }

        public void OnGet(string sortOrder, string search)
        {
            SearchKeyWord = search ?? string.Empty;
            if (!string.IsNullOrEmpty(SearchKeyWord))
                Orders = Orders.Where(order => order.ProductName.Contains(SearchKeyWord, StringComparison.OrdinalIgnoreCase)).ToArray();

            OrdererNameSort = string.IsNullOrEmpty(sortOrder) ? "!on" : "";
            ProductNameSort = sortOrder == "pn" ? "!pn" : "pn";
            PricePerProductSort = sortOrder == "pp" ? "!pp" : "pp";
            TotalPriceSort = sortOrder == "tp" ? "!tp" : "tp";
            CountSort = sortOrder == "c" ? "!c" : "c";
            TimeSort = sortOrder == "t" ? "!t" : "t";

            IEnumerable<Order> orders = Orders;

            switch (sortOrder)
            {
                case "!on":
                    orders = orders.OrderByDescending(order => order.OrdererName);
                    break;
                case "pn":
                    orders = orders.OrderBy(order => order.ProductName);
                    break;
                case "!pn":
                    orders = orders.OrderByDescending(order => order.ProductName);
                    break;
                case "pp":
                    orders = orders.OrderBy(order => order.PricePerProduct);
                    break;
                case "!pp":
                    orders = orders.OrderByDescending(order => order.PricePerProduct);
                    break;
                case "tp":
                    orders = orders.OrderBy(order => order.TotalPrice);
                    break;
                case "!tp":
                    orders = orders.OrderByDescending(order => order.TotalPrice);
                    break;
                case "c":
                    orders = orders.OrderBy(order => order.Count);
                    break;
                case "!c":
                    orders = orders.OrderByDescending(order => order.Count);
                    break;
                case "t":
                    orders = orders.OrderBy(order => order.Time);
                    break;
                case "!t":
                    orders = orders.OrderByDescending(order => order.Time);
                    break;
                default:
                    orders = orders.OrderBy(order => order.OrdererName);
                    break;
            }

            Orders = Orders.ToArray();
        }

        public class Order
        {
            public string OrdererName { get; set; }
            public string ProductName { get; set; }
            public decimal PricePerProduct { get; set; }
            public decimal TotalPrice { get; set; }
            public int Count { get; set; }
            public string Time { get; set; }
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("Products", "Counter", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}