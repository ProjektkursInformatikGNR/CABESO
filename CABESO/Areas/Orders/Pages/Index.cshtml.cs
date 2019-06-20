﻿using CABESO.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace CABESO.Areas.Orders.Pages
{
    /// <summary>
    /// Die Modellklasse der Razor Page zur Übersicht der vom Benutzer aufgegebenen Bestellungen
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
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
        /// Die derzeit offenen Bestellungen
        /// </summary>
        public Order[] Orders { get; set; }

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
            SearchKeyWord = search ?? string.Empty;
            Orders = _context.Orders.Where(order => order.User.Id.Equals(User.GetIdentityUser().Id)).ToArray();

            if (!string.IsNullOrEmpty(SearchKeyWord))
                Orders = Orders.Search(search, order => order.Product.Name).ToArray();

            OrderTimeSort = string.IsNullOrEmpty(sortOrder) ? "!ot" : "";
            ProductNameSort = sortOrder == "p" ? "!p" : "p";
            TotalPriceSort = sortOrder == "tp" ? "!tp" : "tp";
            NumberSort = sortOrder == "n" ? "!n" : "n";
            CollectionTimeSort = sortOrder == "ct" ? "!ct" : "ct";
            CollectionPlaceSort = sortOrder == "cp" ? "!cp" : "cp";

            IOrderedEnumerable<Order> orders = Orders.OrderBy(order => 0);

            switch (sortOrder)
            {
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
            return RedirectToAction("Index", "Orders", new { sortOrder = string.Empty, search = Input.SearchKeyWord?.Trim() ?? string.Empty });
        }
    }
}