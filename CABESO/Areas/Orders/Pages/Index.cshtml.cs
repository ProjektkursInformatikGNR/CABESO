﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    public class IndexModel : PageModel
    {
        public string ProductNameSort { get; set; }
        public string PricePerProductSort { get; set; }
        public string TotalPriceSort { get; set; }
        public string NumberSort { get; set; }
        public string OrderTimeSort { get; set; }
        public string CollectionTimeSort { get; set; }
        public Order[] Orders { get; set; }

        public string SearchKeyWord { get; set; }

        public void OnGet(string sortOrder, string search)
        {
            SearchKeyWord = search ?? string.Empty;

            IEnumerable<Order> EnumerateOrders()
            {
                foreach (Order order in Database.Context.Orders.ToArray())
                    if (order.User.Id.Equals(User.GetIdentityUser().Id))
                        yield return order;
            }
            Orders = EnumerateOrders().ToArray();

            if (!string.IsNullOrEmpty(SearchKeyWord))
                Orders = Orders.Where(order => Program.Matches(order.Product.Name, SearchKeyWord))?.ToArray();

            OrderTimeSort = string.IsNullOrEmpty(sortOrder) ? "!ot" : "";
            ProductNameSort = sortOrder == "p" ? "!p" : "p";
            PricePerProductSort = sortOrder == "ppp" ? "!ppp" : "ppp";
            TotalPriceSort = sortOrder == "tp" ? "!tp" : "tp";
            NumberSort = sortOrder == "n" ? "!n" : "n";
            CollectionTimeSort = sortOrder == "ct" ? "!ct" : "ct";

            IOrderedEnumerable<Order> orders = Orders.OrderBy(order => 0);

            switch (sortOrder)
            {
                case "p":
                    orders = orders.OrderBy(order => order.Product.Name);
                    break;
                case "!p":
                    orders = orders.OrderByDescending(order => order.Product.Name);
                    break;
                case "ppp":
                    orders = orders.OrderBy(order => order.Product.Price);
                    break;
                case "!ppp":
                    orders = orders.OrderByDescending(order => order.Product.Price);
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
                case "ct":
                    orders = orders.OrderBy(order => order.CollectionTime);
                    break;
                case "!ct":
                    orders = orders.OrderByDescending(order => order.CollectionTime);
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

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string SearchKeyWord { get; set; }
        }

        public IActionResult OnPost()
        {
            return RedirectToAction("Index", "Orders", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}